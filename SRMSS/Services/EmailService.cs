using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SRMSS.Models;
using System.IO;

namespace SRMSS.Services
{
    
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }


        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var smtp = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(
                    _settings.SenderEmail,
                    _settings.Password),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(
                    _settings.SenderEmail,
                    _settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            await smtp.SendMailAsync(message);
        }

        public async Task SendEmailWithAttachmentAsync(
    string toEmail,
    string subject,
    string body,
    byte[] attachment,
    string fileName)
        {
            var smtp = new SmtpClient(_settings.SmtpServer)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(
                    _settings.SenderEmail,
                    _settings.Password),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(
                    _settings.SenderEmail,
                    _settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            message.Attachments.Add(
                new Attachment(
                    new MemoryStream(attachment),
                    fileName,
                    "application/pdf"));

            await smtp.SendMailAsync(message);
        }
    }
}