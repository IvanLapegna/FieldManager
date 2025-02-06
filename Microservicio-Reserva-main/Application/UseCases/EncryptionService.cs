using Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class EncryptionService : IEncryptionService
    {
        public string Decrypt(string cipherText)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(cipherText);
      
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public string Encrypt(string plainText)
        {
            
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            
            return Convert.ToBase64String(plainTextBytes);
        }


    }
}
