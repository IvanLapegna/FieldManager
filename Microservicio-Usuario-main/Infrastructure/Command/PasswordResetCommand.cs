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
    public class PasswordResetCommand  : IPasswordResetCommand
    {
        private readonly ApiDbContext _context;
        public PasswordResetCommand(ApiDbContext context) 
        {
         _context = context;
        }

        public async Task Insert(PasswordResetToken token)
        {
            await _context.PasswordResetTokens.AddAsync(token);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(PasswordResetToken token)
        {
            _context.PasswordResetTokens.Remove(token);

            await _context.SaveChangesAsync();
        }
    }
}
