using Application.Interfaces.Service.Authorization;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Jwt
{
    public class JwtService : IAuthTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Generar token de acceso
        
        public async Task<string> GenerateJWTToken(User user)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:key"]!));

            var signingCredentials = new SigningCredentials(
                key: securityKey,
                algorithm: SecurityAlgorithms.HmacSha256Signature
            );

            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()));
            claims.AddClaim(new Claim("IsAdmin", user.IsAdmin.ToString()));
            claims.AddClaim(new Claim("IsActive", user.IsActive.ToString()));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:TokenExpirationMinutes"]!)),   //Tiempo de 1 minuto para pruebas
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            //Creacion del token

            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            var serializedJwt = tokenHandler.WriteToken(tokenConfig);

            return serializedJwt;
        }

        //Generar Refresh Token
        public Task<string> GenerateRefreshToken()
        {
            var size = int.Parse(_configuration["RefreshTokenSettings:Lenght"]!);

            var buffer = new byte[size];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(buffer);

            return Task.FromResult(Convert.ToBase64String(buffer));
        }
        public Task<int> GetRefreshTokenLifetimeInMinutes()
        {
            return Task.FromResult(int.Parse(_configuration["RefreshTokenSettings:LifeTimeInMinutes"]!));
        }

        //Validar Access Token
        public ClaimsPrincipal ValidateAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:key"]!));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        }
    }
}
