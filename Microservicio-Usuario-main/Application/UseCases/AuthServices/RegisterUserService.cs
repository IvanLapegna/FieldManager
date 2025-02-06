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
    public class RegisterUserService : IRegisterUserService
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly IUserCommand _userCommand;
        private readonly IMapper _mapper;
        private readonly IUserQuery _userQuery;

        public RegisterUserService(ICryptographyService cryptographyService, IUserCommand userCommand, IMapper mapper, IUserQuery userQuery)
        {
            _cryptographyService = cryptographyService;
            _userCommand = userCommand;
            _mapper = mapper;
            _userQuery = userQuery;
        }

        public async Task<UserResponse> Register(UserRequest request)
        {
            await CheckEmailExist(request.Email);

            var user = _mapper.Map<User>(request);

            user.Password = await _cryptographyService.HashPassword(request.Password);

            user.IsAdmin = false;

            user.IsActive = true;

            user.ImageUrl = "https://icons.veryicon.com/png/o/internet--web/prejudice/user-128.png";

            await _userCommand.Insert(user);

            return _mapper.Map<UserResponse>(user);
        }
        private async Task CheckEmailExist(string email)
        {
            var emailExist = await _userQuery.ExistEmail(email);

            if (emailExist != null)
            {
                throw new InvalidEmailException("El correo electrónico ingresado ya está registrado.");
            }
        }
    }
}
