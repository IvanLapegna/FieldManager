using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service.Authorization
{
    public interface IAuthTokenService
    {
        Task<string> GenerateJWTToken(User user);
        Task<string> GenerateRefreshToken();
        Task<int> GetRefreshTokenLifetimeInMinutes();
        ClaimsPrincipal ValidateAccessToken(string token);
    }
}
