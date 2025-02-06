using Application.Dtos.Request;
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
    public class UserQuery : IUserQuery
    {
        private readonly ApiDbContext _context;

        public UserQuery(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var list = await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.UserID)
                .ToListAsync();

            return list;
        }

        public async Task<User> GetByIdUser(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == id);

            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> ExistEmail(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);  

            return result;
        }
    }
}
