using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } // Email del usuario
        public string Token { get; set; } // Código de restablecimiento
        public DateTime Expiration { get; set; } // Tiempo de expiración
    }

}
