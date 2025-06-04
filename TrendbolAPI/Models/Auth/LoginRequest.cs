using System.ComponentModel.DataAnnotations;

namespace TrendbolAPI.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        public string Password { get; set; } = null!;
    }
} 