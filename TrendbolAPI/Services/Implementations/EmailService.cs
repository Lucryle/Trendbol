using System.Net.Mail;
using System.Net;
using TrendbolAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace TrendbolAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private static readonly Dictionary<string, (string Code, DateTime Expiry)> _verificationCodes = new();

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _smtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new InvalidOperationException("SMTP Server not found in configuration");
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? throw new InvalidOperationException("SMTP Port not found in configuration"));
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? throw new InvalidOperationException("SMTP Username not found in configuration");
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? throw new InvalidOperationException("SMTP Password not found in configuration");
            _fromEmail = _configuration["EmailSettings:FromEmail"] ?? throw new InvalidOperationException("From Email not found in configuration");
        }

        private async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
                };

                using var message = new MailMessage(_fromEmail, to, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
                _logger.LogInformation($"Email successfully sent to {to}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendVerificationCodeAsync(string email)
        {
            var code = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(10);

            _verificationCodes[email] = (code, expiry);
            var subject = "Email Doğrulama Kodu";
            var body = $@"
                <h2>Trendbol'a Hoş Geldiniz!</h2>
                <p>Email adresinizi doğrulamak için aşağıdaki kodu kullanın:</p>
                <h1 style='color: #4CAF50;'>{code}</h1>
                <p>Bu kod 10 dakika süreyle geçerlidir.</p>
                <p>Eğer bu işlemi siz yapmadıysanız, lütfen bu emaili dikkate almayın.</p>";

            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            if (!_verificationCodes.TryGetValue(email, out var verificationData))
                return false;

            if (verificationData.Code != code)
                return false;

            if (verificationData.Expiry < DateTime.UtcNow)
                return false;

            _verificationCodes.Remove(email);
            return true;
        }
    }
} 