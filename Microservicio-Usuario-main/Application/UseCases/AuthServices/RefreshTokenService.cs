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
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IAuthTokenService _authTokenService;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenCommand _refreshTokenCommand;
        private readonly IRefreshTokenQuery _refreshTokenQuery;
        private readonly IUserQuery _userQuery;

        public RefreshTokenService(IAuthTokenService authTokenService, IMapper mapper, IRefreshTokenCommand refreshTokenCommand, IRefreshTokenQuery refreshTokenQuery, IUserQuery userQuery)
        {
            _authTokenService = authTokenService;
            _mapper = mapper;
            _refreshTokenCommand = refreshTokenCommand;
            _refreshTokenQuery = refreshTokenQuery;
            _userQuery = userQuery;
        }

        public async Task<LoginResponse> RefreshAccessToken(RefreshTokenRequest request)
        {
            var refreshToken = await _refreshTokenQuery.GetByToken(request.RefreshToken);

            if (refreshToken == null || !refreshToken.IsActive || refreshToken.ExpireDate < DateTime.UtcNow)
            {
                throw new InvalidRefreshTokenException("El Refresh Token ha expirado o es inválido.");
            }

            //Traigo al usuario asociado al token para generarle un nuevo Acess Token (Y ademas rotar el refresh token como buena practica de seguridad)

            var user = await _userQuery.GetByIdUser(refreshToken.UserId);

            if (user == null)
            {
                throw new InvalidValueException("No se encontró al usuario.");
            }

            var accessToken = await _authTokenService.GenerateJWTToken(user);

            //Roto el refreshToken Antiguo
            var newRefreshToken = new RefreshToken
            {
                Token = await _authTokenService.GenerateRefreshToken(),
                CreateDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddMinutes(await _authTokenService.GetRefreshTokenLifetimeInMinutes()),
                UserId = user.UserID,
                IsActive = true
            };

            await _refreshTokenCommand.Delete(refreshToken);

            await _refreshTokenCommand.Insert(newRefreshToken);

            return new LoginResponse { Token = accessToken, RefreshToken = newRefreshToken.Token, Result = true, Message = "Tokens renovados exitosamente." };
        }
    }
}
