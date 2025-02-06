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
    public class UserCommand : IUserCommand
    {
        private readonly ApiDbContext _context;

        public UserCommand(ApiDbContext context)
        { 
            _context = context;
        }

        public async Task Insert(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }
    }
}
