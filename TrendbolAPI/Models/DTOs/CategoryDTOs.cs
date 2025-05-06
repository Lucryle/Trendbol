using System.ComponentModel.DataAnnotations;

namespace TrendbolAPI.Models.DTOs
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Kategori adı 2-50 karakter arasında olmalıdır.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori açıklaması zorunludur.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Kategori açıklaması 10-200 karakter arasında olmalıdır.")]
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateCategoryDTO
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Kategori adı 2-50 karakter arasında olmalıdır.")]
        public string? Name { get; set; }

        [StringLength(200, MinimumLength = 10, ErrorMessage = "Kategori açıklaması 10-200 karakter arasında olmalıdır.")]
        public string? Description { get; set; }
    }

    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
} 