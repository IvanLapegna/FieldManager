using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Interfaces.Service;
using Application.Interfaces.Service.Authorization;
using Application.Interfaces.Service.Cryptography;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AuthServices
{
    public class LoginService : ILoginService
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly IAuthTokenService _authTokenService;
        private readonly IMapper _mapper;
        private readonly IUserQuery _userQuery;
        private readonly IRefreshTokenCommand _refreshTokenCommand;

        public LoginService(ICryptographyService cryptographyService, IAuthTokenService authTokenService, IMapper mapper, IUserQuery userQuery, IRefreshTokenCommand refreshTokenCommand)
        {
            _cryptographyService = cryptographyService;
            _authTokenService = authTokenService;
            _mapper = mapper;
            _userQuery = userQuery;
            _refreshTokenCommand = refreshTokenCommand;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userQuery.ExistEmail(request.Email);

            if (user == null)
            {
                throw new InvalidEmailException("Los campos ingresados no son válidos.");
            }

            if (!user.IsActive) 
            {
                throw new InactiveUserException("La cuenta de usuario está inactiva. Por favor, contacte con soporte.");
            }

            var passwordCorrect = await _cryptographyService.VerifyPassword(user.Password, request.Password);

            if (!passwordCorrect)
            {
                throw new InvalidPasswordException("Los campos ingresados no son válidos.");
            }

            //Generacion de Refresh Token

            var refreshToken = new RefreshToken {
                Token = await _authTokenService.GenerateRefreshToken(),
                CreateDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddMinutes(await _authTokenService.GetRefreshTokenLifetimeInMinutes()),
                UserId = user.UserID,
                IsActive = true
                };

            await _refreshTokenCommand.Insert(refreshToken);

            var accessToken = await _authTokenService.GenerateJWTToken(user);

            return new LoginResponse { Token = accessToken, RefreshToken = refreshToken.Token, Result = true, Message = "OK" };
        }
    }
}
