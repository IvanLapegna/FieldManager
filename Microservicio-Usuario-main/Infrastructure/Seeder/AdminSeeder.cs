using Application.Interfaces.Service.Cryptography;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeder
{
    public class AdminSeeder
    {
        private readonly ApiDbContext _context;
        private readonly ICryptographyService _cryptographyService;

        public AdminSeeder(ApiDbContext context, ICryptographyService cryptographyService)
        {
            _context = context;
            _cryptographyService = cryptographyService;
        }

        public async Task SeedAsync()
        {
            var adminEmail1 = "admin1@fieldmanager.com";
            var adminEmail2 = "admin2@fieldmanager.com";
            var adminEmail3 = "admin3@fieldmanager.com";

            var admin1Exists = await _context.Users.AnyAsync(u => u.Email == adminEmail1);
            var admin2Exists = await _context.Users.AnyAsync(u => u.Email == adminEmail2);
            var admin3Exists = await _context.Users.AnyAsync(u => u.Email == adminEmail3);

            if (!admin1Exists)
            {
                var adminPassword1 = await _cryptographyService.HashPassword("Prueba123");
                _context.Users.Add(new Domain.Models.User { Name = "Admin1", Email = adminEmail1, Password = adminPassword1, IsAdmin = true, IsActive = true, ImageUrl = "https://icons.veryicon.com/png/o/internet--web/prejudice/user-128.png" });
            }

            if (!admin2Exists)
            {
                var adminPassword2 = await _cryptographyService.HashPassword("0nrGLaao");
                _context.Users.Add(new Domain.Models.User { Name = "Admin2", Email = adminEmail2, Password = adminPassword2, IsAdmin = true, IsActive = true, ImageUrl = "https://icons.veryicon.com/png/o/internet--web/prejudice/user-128.png" });
            }

            if (!admin3Exists)
            {
                var adminPassword3 = await _cryptographyService.HashPassword("TCfc7x9H");
                _context.Users.Add(new Domain.Models.User { Name = "Admin3", Email = adminEmail3, Password = adminPassword3, IsAdmin = true, IsActive = true, ImageUrl = "https://icons.veryicon.com/png/o/internet--web/prejudice/user-128.png" });
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
