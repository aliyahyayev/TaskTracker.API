using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Dtos
{
    // Task məlumatlarını istifadəçiyə göstərmək üçün istifadə ediləcək DTO
    public record TodoTaskDto(
        int Id,
        string Title,
        string Description,
        bool IsCompleted,
        DateTime CreatedAt,
        DateTime? DueDate,
        int CategoryId,
        string CategoryName
    );

    public record CreateTodoTaskDto(
        [Required(ErrorMessage = "Tapşırıq başlığı mütləqdir.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Başlıq minimum 5, maksimum 100 simvol ola bilər.")]
        string Title,

        [StringLength(500, ErrorMessage = "Təsvir maksimum 500 simvol ola bilər.")]
        string Description,

        DateTime? DueDate,

        [Required(ErrorMessage = "Kateqoriya ID mütləqdir.")]
        int CategoryId //burda dropdown list ile categoryid göndəriləcək
    );

    public record UpdateTodoTaskDto(
        [Required(ErrorMessage = "Tapşırıq başlığı mütləqdir.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Başlıq minimum 5, maksimum 100 simvol ola bilər.")]
        string Title,

        [StringLength(500, ErrorMessage = "Təsvir maksimum 500 simvol ola bilər.")]
        string Description,

        bool IsCompleted,

        DateTime? DueDate,

        [Required(ErrorMessage = "Kateqoriya ID mütləqdir.")]
        int CategoryId
    );
}