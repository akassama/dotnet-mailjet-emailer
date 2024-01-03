using EmailerTestApp.Models;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;

namespace EmailerTestApp.Repository.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly string _apiKey;
        private readonly string _secretKey;
        public EmailService(ILogger<EmailService> logger, IConfiguration configuration) 
        {
            _logger = logger;
            _apiKey = configuration["SystemConfiguration:MailjetApiKey"];
            _secretKey = configuration["SystemConfiguration:MailjetSecretKey"];
        }

        public async Task<bool> SendEmail(EmailModel emailModel)
        {
            try
            {
                MailjetClient client = new MailjetClient(_apiKey, _secretKey);

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource
                };

                //format email
                emailModel.Body = await FormatEmailTemplate(emailModel.From, emailModel.Subject, emailModel.ToEmail, emailModel.Body);

                var email = new TransactionalEmailBuilder()
                       .WithFrom(new SendContact(emailModel.From))
                       .WithSubject(emailModel.Subject)
                       .WithHtmlPart(emailModel.Body)
                       .WithTo(new SendContact(emailModel.ToEmail))
                       .Build();

                var response = await client.SendTransactionalEmailAsync(email);
                var message = response.Messages[0];

                bool result = message.Status.ToLower() == "success";

                _logger.LogInformation("Email sent successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email.");

                return false;
            }
        }

        public async Task<string> FormatEmailTemplate(string fromEmail, string subject, string toEmail, string message)
        {
            string filePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"Files/email.html"));
            string template = File.ReadAllText($"{filePath}");

            template = template.Replace("{fromEmail}", fromEmail);
            template = template.Replace("{subject}", subject);
            template = template.Replace("{message}", message);

            return template;
        }
    }
}
