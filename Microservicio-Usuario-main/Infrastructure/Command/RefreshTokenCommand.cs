using Application.Interfaces.Command;
using Domain.Models;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Command
{
    public class RefreshTokenCommand : IRefreshTokenCommand
    {
        private readonly ApiDbContext _context;

        public RefreshTokenCommand(ApiDbContext context)
        {
            _context = context;
        }

        public async Task Insert(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);

            await _context.SaveChangesAsync();
        }
    }
}
