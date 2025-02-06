using Application.Dtos.Request;
using Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IUserService
    {
        Task<UserResponse> UpdateUser(int id, UserUpdateRequest request);
        Task<UserResponse> GetUserById(int id);
        Task<List<UserResponse>> GetAllUsers();
        Task<GenericUserResponse> DeleteUser(int id);
        Task<UserResponse> RemoveUserImage(int id);
    }
}
