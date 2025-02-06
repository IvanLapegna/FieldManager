using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Interfaces.Service;
using Application.Interfaces.Service.Authorization;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AuthServices
{
    public class SignOutService : ISignOutService
    {
        private readonly IAuthTokenService _authTokenService;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenCommand _refreshTokenCommand;
        private readonly IRefreshTokenQuery _refreshTokenQuery;

        public SignOutService(IAuthTokenService authTokenService, IMapper mapper, IRefreshTokenCommand refreshTokenCommand, IRefreshTokenQuery refreshTokenQuery)
        {
            _authTokenService = authTokenService;
            _mapper = mapper;
            _refreshTokenCommand = refreshTokenCommand;
            _refreshTokenQuery = refreshTokenQuery;
        }

        public async Task<GenericUserResponse> SignOut(SignOutRequest request)
        {
            //Se debe eliminar el refreshToken para desloguear de dicha seccion.
            var refreshToken = await _refreshTokenQuery.GetByToken(request.RefreshToken);

            if (refreshToken == null)
            {
                throw new InvalidRefreshTokenException("No se encontró el Refresh Token.");
            }

            await _refreshTokenCommand.Delete(refreshToken);

            return new GenericUserResponse { Message = "Cierre de sesión exitoso." };
        }
    }
}
