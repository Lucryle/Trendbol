using TrendbolAPI.Models;

namespace TrendbolAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendVerificationCodeAsync(string email);
        Task<bool> VerifyCodeAsync(string email, string code);
    }
} 