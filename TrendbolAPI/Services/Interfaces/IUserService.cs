using TrendbolAPI.Models;

namespace TrendbolAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> IsEmailAvailableAsync(string email);
        Task<User?> ValidateUserAsync(string email, string password);
        Task<bool> SendVerificationCode(string email);
        Task<bool> VerifyEmail(string email, string code);
    }
} 