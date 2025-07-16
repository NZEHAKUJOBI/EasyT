using API.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly string _emailAddress;
        private readonly string _appPassword;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _emailAddress = configuration["EmailSettings:Email_Address"] 
                ?? throw new ArgumentNullException("Email Address is not configured");
            _appPassword = configuration["EmailSettings:Email_App_Password"] 
                ?? throw new ArgumentNullException("Email App Password is not configured");
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string message,
         string fromEmail = null, byte[] pdfAttachment = null)
        {
            try
            {
                var sender = fromEmail ?? _emailAddress;

                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(_emailAddress, _appPassword),
                    EnableSsl = true
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(sender),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mail.To.Add(toEmail);

                if (pdfAttachment != null)
                {
                    var attachment = new Attachment(new MemoryStream(pdfAttachment), "attachment.pdf", "application/pdf");
                    mail.Attachments.Add(attachment);
                }

                await smtpClient.SendMailAsync(mail);
                _logger.LogInformation("Email sent to {ToEmail}", toEmail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {ToEmail}", toEmail);
                return false;
            }
        }
    }
}
