using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Query
{
    public interface IPasswordResetQuery
    {
        Task<PasswordResetToken> GetByEmailAndCodeAsync(string email, string resetCode);
    }
}
