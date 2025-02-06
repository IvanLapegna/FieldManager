using Application.Interfaces.Query;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class RefreshTokenQuery : IRefreshTokenQuery
    {
        private readonly ApiDbContext _context;

        public RefreshTokenQuery(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == token);

            return refreshToken;
        }

        public async Task<List<RefreshToken>> GetAllTokensByUserID(int userID)
        {
            var list = await _context.RefreshTokens
                .AsNoTracking()
                .Where(r => r.UserId == userID)
                .ToListAsync();

            return list;
        }
    }
}
