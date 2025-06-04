using System.ComponentModel.DataAnnotations;

namespace TrendbolAPI.Models
{
    public class ConfirmEmailRequest
    {
        [Required(ErrorMessage = "Email alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Doğrulama kodu zorunludur")]
        public string VerificationCode { get; set; } = null!;
    }
} 