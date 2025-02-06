using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(UserResponse))]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [Authorize]
        [HttpGet("/api/v1/User/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(UserResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(ApiError))]
        public async Task<IActionResult> GetByIdUser(int id)
        {
            try
            {
                var result = await _userService.GetUserById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (InvalidValueException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
        }

        [Authorize]
        [HttpDelete("/api/v1/User/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(GenericUserResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ApiError))]
        [ProducesResponseType(statusCode: 404, type: typeof(ApiError))]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (InvalidValueException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
            catch (InactiveUserException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [Authorize]
        [HttpPut("/api/v1/User/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(UserResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ApiError))]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateRequest request)
        {
            try
            {
                var result = await _userService.UpdateUser(id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (InvalidValueException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
            catch (InvalidEmailException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [Authorize]
        [HttpPatch("/api/v1/User/RemoveImage/{id}")]
        [ProducesResponseType(statusCode: 200, type: typeof(UserResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(ApiError))]

        public async Task<IActionResult> RemoveUserImage (int id)
        {
            try
            {
                var result = await _userService.RemoveUserImage(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (InvalidValueException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };
            }
        }
    }
}
