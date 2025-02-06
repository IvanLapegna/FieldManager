using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public int UserID { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
