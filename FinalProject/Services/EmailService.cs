using System.Net;
using System.Net.Mail;

namespace FinalProject.Services
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        public async Task SendVerificationEmail(string toEmail, string verificationLink)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_smtpUser);
                message.To.Add(new MailAddress(toEmail));
                message.Subject = "Email Verification";
                message.Body = $"Please verify your email by clicking the following link: {verificationLink}";
                message.IsBodyHtml = true;

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                    client.EnableSsl = true;

                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
