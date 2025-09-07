
namespace API.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string message, string fromEmail = null, byte[] pdfAttachment = null);
    }
}
