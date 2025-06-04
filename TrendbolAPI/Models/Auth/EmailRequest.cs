using System.ComponentModel.DataAnnotations;

namespace TrendbolAPI.Models
{
    public class EmailRequest
    {
        [Required(ErrorMessage = "Email alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; } = null!;
    }
} 