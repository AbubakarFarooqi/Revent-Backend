using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Revent.Common.CommonModels;
using Revent.Services.IServices;

namespace Revent.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public void SendMail(string subject, string body, string to)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(name: "Revent", address: _emailSettings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;
                message.Body = new TextPart("plain") { Text = body };

                using var smtp = new SmtpClient();
                smtp.Connect(_emailSettings.SMTPServer, _emailSettings.SMTPPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                smtp.Send(message);
                smtp.Disconnect(true);

                Console.WriteLine($"Email sent successfully to ${to}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
