using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Query
{
    public interface IUserQuery
    {
        Task<User> ExistEmail(string email);
        Task<User> GetByIdUser(int id);
        Task<List<User>> GetAllUsers();
        Task<User> GetByEmailAsync(string email);
    }
}
