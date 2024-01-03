using EmailerTestApp.Models;

namespace EmailerTestApp.Repository.EmailService
{
    public interface IEmailService
    {
        public Task<bool> SendEmail(EmailModel emailModel);

        public Task<string> FormatEmailTemplate(string fromEmail, string subject, string toEmail, string message);
    }
}
