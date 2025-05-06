using System.ComponentModel.DataAnnotations;

namespace TrendbolAPI.Models.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur")]
        public string Password { get; set; }
    }
} 