using Application.Dtos.Request;
using Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IPasswordResetService
    {
        Task GenerateResetCodeAsync(string email);
        Task<bool> ValidateResetCodeAndChangePasswordAsync(string email, string resetCode, string newPassword);
        Task<GenericUserResponse> PasswordChange(PasswordChangeRequest request);
    }
}
