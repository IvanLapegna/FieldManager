using Application.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Application.UseCases.AuthServices
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587; // Puerto SMTP para TLS
        private readonly string _senderEmail = "fieldmanager2024@gmail.com";
        private readonly string _senderPassword = "tyrr zclo zeer igrl"; // Contraseña de aplicación de Gmail

        public async Task SendPasswordResetEmailAsync(string email, string resetCode)
        {
            
            string subject = "Field Manager | Código de restablecimiento de contraseña";
            string body = $"Se ha solicitado el restablecimiento de su contraseña. En caso de no haber sido usted, desestime este mail. \n \n Su código para restablecer la contraseña es: {resetCode}";

            using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage(_senderEmail, email, subject, body);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (SmtpException ex)
                {
           
                    throw new Exception("Error al enviar el correo electrónico", ex);
                }
            }
        }
    }
}
