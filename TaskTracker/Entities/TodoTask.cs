using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Entities
{
    public class TodoTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)] // Tapşırıq başlığı minimum 5 simvol olmalıdır
        public string Title { get; set; } = string.Empty;

        [StringLength(500)] // Təsvir maksimum 500 simvol ola bilər
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false; // Tapşırığın icra edilib-edilməməsi

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Yaradılma tarixi

        public DateTime? DueDate { get; set; } // Son icra tarixi (null ola bilər)

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; } 
    }
}