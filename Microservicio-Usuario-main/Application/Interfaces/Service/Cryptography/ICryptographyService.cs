using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service.Cryptography
{
    public interface ICryptographyService
    {
        Task<string> GenerateSalt();  //Se le agrega aleatoriedad al Hash
        Task<string> HashPassword(string password);
        Task<bool> VerifyPassword(string hashedPassword, string password);
    }
}
