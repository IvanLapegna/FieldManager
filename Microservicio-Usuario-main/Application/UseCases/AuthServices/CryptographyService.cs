using Application.Interfaces.Service.Cryptography;
using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Application.UseCases.AuthServices
{
    public class CryptographyService : ICryptographyService
    {
        private const int _saltSize = 16;
        private const int _hashSize = 32;

        public CryptographyService() { }

        public Task<string> GenerateSalt()
        {
            var buffer = new byte[_saltSize];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(buffer);

            return Task.FromResult(Convert.ToBase64String(buffer));
        }

        public async Task<string> HashPassword(string password)
        {
            var salt = await GenerateSalt();

            //Hasheo utilizando Argon2 (Para una seguridad mas robusta).

            using var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password))
            {
                DegreeOfParallelism = 8,
                MemorySize = 8192,
                Iterations = 40,
                Salt = Encoding.UTF8.GetBytes(salt)
            };

            var hash = argon2.GetBytes(_hashSize);

            // Concatenar salt y hash en una sola cadena

            var combinedHash = $"{salt}.{Convert.ToBase64String(hash)}";

            return combinedHash;
        }

        public Task<bool> VerifyPassword(string hashedPassword, string password)
        {
            // Separar salt y hash almacenados
            var parts = hashedPassword.Split('.');
            if (parts.Length != 2)
                throw new FormatException("The format of the stored hash is incorrect.");

            var salt = parts[0];
            var storedPasswordHash = parts[1];

            // Hashear la contraseña ingresada usando el salt almacenado

            using var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password))
            {
                DegreeOfParallelism = 8,
                MemorySize = 8192,
                Iterations = 40,
                Salt = Encoding.UTF8.GetBytes(salt)
            };

            var hash = argon2.GetBytes(_hashSize);
            var enteredPasswordHash = Convert.ToBase64String(hash);

            // Comparar el hash generado con el hash almacenado
            return Task.FromResult(enteredPasswordHash.Equals(storedPasswordHash));
        }
    }
}
