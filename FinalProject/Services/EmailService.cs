using MimeKit;
//using System.Net.Mail;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace FinalProject.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Your App", _configuration["EmailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration["EmailSettings:SMTPServer"], int.Parse(_configuration["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:Password"]);

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
