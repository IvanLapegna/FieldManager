using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces.Service;
using Application.UseCases.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterUserService _registerUserService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ISignOutService _signOutService;
        private readonly IPasswordResetService _passwordResetService;

        public AuthController(ILoginService loginService, IRegisterUserService registerUserService, IRefreshTokenService refreshTokenService, ISignOutService signOutService, IPasswordResetService passwordResetService)
        {
            _loginService = loginService;
            _registerUserService = registerUserService;
            _refreshTokenService = refreshTokenService;
            _signOutService = signOutService;
            _passwordResetService = passwordResetService;

        }

        [HttpPost("/api/v1/Auth/Register")]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: 201, type: typeof(UserResponse))]
        [ProducesResponseType(statusCode: 409, type: typeof(ApiError))]
        public async Task<IActionResult> Register(UserRequest request)
        {
            try
            {
                var result = await _registerUserService.Register(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (InvalidEmailException ex) 
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 409 };
            }

        }

        [HttpPost("/api/v1/Auth/Login")]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: 200, type: typeof(LoginResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ApiError))]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _loginService.Login(request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (InvalidPasswordException ex) 
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (InvalidEmailException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (InactiveUserException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [HttpPost("/api/v1/Auth/RefreshToken")]
        [ProducesResponseType(statusCode: 200, type: typeof(LoginResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ApiError))]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var result = await _refreshTokenService.RefreshAccessToken(request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (InvalidRefreshTokenException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (InvalidValueException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [HttpPost("/api/v1/Auth/SignOut")]
        [ProducesResponseType(statusCode: 200, type: typeof(GenericUserResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ApiError))]
        public async Task<IActionResult> SignOut(SignOutRequest request)
        {
            try
            {
                var result = await _signOutService.SignOut(request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (InvalidRefreshTokenException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [Authorize]
        [HttpPost("/api/v1/Auth/ChangePassword")]
        [ProducesResponseType(statusCode: 200, type: typeof(GenericUserResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ApiError))]
        public async Task<IActionResult> ChangePassword(PasswordChangeRequest request)
        {
            try
            {
                var result = await _passwordResetService.PasswordChange(request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (InvalidPasswordException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (InvalidValueException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return new JsonResult(new ApiError { Message = "La dirección de correo electrónico es obligatoria." }) { StatusCode = 400};
            }

            await _passwordResetService.GenerateResetCodeAsync(request.Email);
            return new JsonResult(new GenericUserResponse { Message = "El código de restablecimiento ha sido enviado." }) { StatusCode = 200 };
        }
    
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateResetCodeAndChangePassword([FromBody] PasswordResetValidationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.ResetCode) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return new JsonResult(new ApiError { Message = "El correo electrónico, el código de restablecimiento y la nueva contraseña son obligatorios." }) { StatusCode = 400 };
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 8)
            {
                return new JsonResult(new ApiError { Message = "La nueva contraseña debe tener al menos 8 caracteres." }) { StatusCode = 400 };
            }
    
            var result = await _passwordResetService.ValidateResetCodeAndChangePasswordAsync(request.Email, request.ResetCode, request.NewPassword);
            if (!result)
            {
                return new JsonResult(new ApiError { Message = "El código de restablecimiento es inválido o ha expirado." }) { StatusCode = 400 };
            }

            return new JsonResult(new GenericUserResponse { Message = "La contraseña ha sido actualizada exitosamente." }) { StatusCode = 200 };
        }
    }
}
