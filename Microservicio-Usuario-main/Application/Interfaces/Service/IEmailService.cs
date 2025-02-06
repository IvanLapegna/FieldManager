using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Application.Interfaces.Service
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetCode);
    }
}
