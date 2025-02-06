using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Interfaces.Service;
using Application.Interfaces.Service.Authorization;
using Application.Interfaces.Service.Cryptography;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Application.UseCases
{
    public class UserService : IUserService
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly IUserCommand _command;
        private readonly IMapper _mapper;
        private readonly IUserQuery _query;
        private readonly IRefreshTokenQuery _refreshTokenQuery;
        private readonly IRefreshTokenCommand _refreshTokenCommand;

        public UserService(IUserCommand command, IMapper mapper, IUserQuery query, ICryptographyService cryptographyService, IRefreshTokenQuery refreshTokenQuery, IRefreshTokenCommand refreshTokenCommand)
        {
            _command = command;
            _mapper = mapper;
            _query = query;
            _cryptographyService = cryptographyService;
            _refreshTokenQuery = refreshTokenQuery;
            _refreshTokenCommand = refreshTokenCommand;
        }
        public async Task<UserResponse> UpdateUser(int id, UserUpdateRequest request)
        {
            var user = await _query.GetByIdUser(id);

            if (user == null)
            {
                throw new InvalidValueException("No se encontró ningún usuario con el ID  " + id);
            }

            //Actualiza el mail unicamente si no es vacio, no es el mismo que ya posee, y si no lo posee otro usuario.

            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
            {
                await CheckEmailExist(id, request.Email);
                user.Email = request.Email; 
            }

            //Actualiza el nombre unicamente si esta presente y no es el mismo al ya ingresado
            if (!string.IsNullOrEmpty(request.Name) && request.Name != user.Name)
            {
                user.Name = request.Name;
            }

            if (!string.IsNullOrEmpty(request.ImageURL) && request.ImageURL != user.ImageUrl)
            {
                user.ImageUrl = request.ImageURL;
            }

            await _command.Update(user);

            var result = await _query.GetByIdUser(user.UserID);

            return _mapper.Map<UserResponse>(result);
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            var user = await _query.GetByIdUser(id);

            if (user == null)
            {
                throw new InvalidValueException("No se encontró ningún usuario con el ID  " + id);
            }

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            var list = await _query.GetAllUsers();

            return _mapper.Map<List<UserResponse>>(list);
        }

        public async Task<GenericUserResponse> DeleteUser(int id)
        {
            var user = await _query.GetByIdUser(id);

            if (user == null)
            {
                throw new InvalidValueException("No se encontró ningún usuario con el ID " + id);
            }

            if (!user.IsActive)
            {
                throw new InactiveUserException("El usuario ya está inactivo.");
            }

            //Si esta inactivo es como si se borrara el user

            user.IsActive = false;

            await _command.Update(user); 

            //Se deben eliminar ademas todos los refreshTokens almacenados del user

            var refreshTokens = await _refreshTokenQuery.GetAllTokensByUserID(user.UserID);

            foreach (var refreshToken in refreshTokens)
            {
                await _refreshTokenCommand.Delete(refreshToken);
            }

            return new GenericUserResponse { Message = "El usuario con ID " + id + " ha sido eliminado exitosamente." };
        }

        public async Task<UserResponse> RemoveUserImage(int id)
        {
            var user = await _query.GetByIdUser(id);

            if (user == null) 
            {
                throw new InvalidValueException("No se encontró ningún usuario con el ID " + id);
            }

            user.ImageUrl = "https://icons.veryicon.com/png/o/internet--web/prejudice/user-128.png"; //No tiene mas imagen, se le asigna la URL de una imagen de usuario generica

            await _command.Update(user);

            var result = await _query.GetByIdUser(user.UserID);

            return _mapper.Map<UserResponse>(result);
        }

        private async Task CheckEmailExist(int id, string email)
        {
            var emailExist = await _query.ExistEmail(email);

            if (emailExist != null && emailExist.UserID != id)
            {
                throw new InvalidEmailException("El correo electrónico ingresado ya está registrado.");
            }
        }
    }
}
