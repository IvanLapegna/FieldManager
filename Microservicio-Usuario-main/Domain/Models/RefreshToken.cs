using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RefreshToken
    {
        public int RefreshTokenID { get; set; }
        public string Token { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
