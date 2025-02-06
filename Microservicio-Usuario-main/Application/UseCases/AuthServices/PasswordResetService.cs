using Application.Interfaces.Service;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Application.Interfaces.Service.Cryptography;
using Application.Dtos.Response;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Microsoft.AspNetCore.Http;
using Application.Dtos.Request;
using System.Security.Claims;
using Application.Exceptions;



namespace Application.UseCases.AuthServices
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IEmailService _emailService;
        private readonly ICryptographyService _cryptographyService;
        private readonly IUserCommand _userCommand;
        private readonly IUserQuery _userQuery;
        private readonly IPasswordResetCommand _passwordResetCommand;
        private readonly IPasswordResetQuery _passwordResetQuery;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PasswordResetService(IEmailService emailService, IPasswordResetCommand passwordResetCommand, IPasswordResetQuery passwordResetQuery, ICryptographyService cryptographyService, IUserCommand userCommand, IUserQuery userQuery, IHttpContextAccessor httpContextAccessor)
        {
            _emailService = emailService;
            _cryptographyService = cryptographyService;
            _userCommand = userCommand;
            _userQuery = userQuery;
            _passwordResetCommand = passwordResetCommand;
            _passwordResetQuery = passwordResetQuery;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task GenerateResetCodeAsync(string email)
        {
            // Generar un código de 6 dígitos
            string resetCode = Guid.NewGuid().ToString().Substring(0, 6);

            // Crear un nuevo token con expiración de 15 minutos
            var passwordResetToken = new PasswordResetToken
            {
                Email = email,
                Token = resetCode,
                Expiration = DateTime.UtcNow.AddMinutes(10) // Código válido por 10 minutos
            };

            // Guardar en la base de datos
            await _passwordResetCommand.Insert(passwordResetToken);

            // Enviar el código por correo
            await _emailService.SendPasswordResetEmailAsync(email, resetCode);
        }


        public async Task<bool> ValidateResetCodeAndChangePasswordAsync(string email, string resetCode, string newPassword)
        {
            var token = await _passwordResetQuery.GetByEmailAndCodeAsync(email, resetCode);
            if (token == null || token.Expiration < DateTime.UtcNow)
            {
                return false; // El código no es válido o ha expirado
            }
        
            var user = await _userQuery.GetByEmailAsync(email);
            if (user == null)
            {
                return false; // El usuario no existe
            }

            // Hashear la nueva contraseña
            var hashedPassword = _cryptographyService.HashPassword(newPassword);

            // Actualizar la contraseña del usuario
            user.Password = await hashedPassword;

            // Guardar cambios en la base de datos
            await _userCommand.Update(user);

            // Eliminar el token de restablecimiento una vez usado
            await _passwordResetCommand.Delete(token);

            return true;
        }


        //metodo para restablecer contraseña dentro del sistema

        public async Task<GenericUserResponse> PasswordChange(PasswordChangeRequest request)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            //Valido que se tenga la ID del usuario en el claim

            var userIdString = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdString == null || !int.TryParse(userIdString, out var userId))
            {
                throw new InvalidValueException("El usuario no está autenticado o el ID de usuario es inválido.");
            }

            var user = await _userQuery.GetByIdUser(userId);
            if (user == null)
            {
                throw new InvalidValueException("El usuario no fue encontrado.");
            }

            bool isPasswordValid = await _cryptographyService.VerifyPassword(user.Password, request.CurrentPassword);
            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("La contraseña actual es incorrecta.");
            }

            user.Password = await _cryptographyService.HashPassword(request.NewPassword);
            
            await _userCommand.Update(user);

            return new GenericUserResponse { Message = "¡Tu contraseña ha sido cambiada exitosamente!" };
        }
    }
}
