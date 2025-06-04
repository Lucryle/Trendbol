using Microsoft.EntityFrameworkCore;
using TrendbolAPI.Data;
using TrendbolAPI.Models;
using TrendbolAPI.Repositories.Interfaces;
using TrendbolAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace TrendbolAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private static readonly Dictionary<string, (string Code, DateTime Expiry)> _verificationCodes = new();

        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            return user;
        }

        public async Task<bool> SendVerificationCode(string email)
        {
            return await _emailService.SendVerificationCodeAsync(email);
        }

        public async Task<bool> VerifyEmail(string email, string code)
        {
            return await _emailService.VerifyCodeAsync(email, code);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 