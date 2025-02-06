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
    public class PasswordResetQuery : IPasswordResetQuery
    {
        private readonly ApiDbContext _context;

        public PasswordResetQuery(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordResetToken> GetByEmailAndCodeAsync(string email, string resetCode)
        {
            return await _context.PasswordResetTokens.FirstOrDefaultAsync(t => t.Email == email && t.Token == resetCode);
        }
    }
}
