using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Dtos
{
    //id ve name property-lərini saxlayan DTO
    public record CategoryDto(int Id, string Name);

    // Yeni Kateqoriya yaradarkən istifadə olunacaq DTO (İnput Validation ilə)
    public record CreateCategoryDto(
        [Required(ErrorMessage = "Kateqoriya adı mütləqdir.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kateqoriya adı 3 ilə 50 simvol arasında olmalıdır.")]
        string Name
    );
}