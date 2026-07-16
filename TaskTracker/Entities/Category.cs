using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Entities
{
    public class Category
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        [StringLength(50, MinimumLength = 3)] // Minimum 3, maksimum 50 simvol ola bilər
        public string Name { get; set; } = string.Empty;

        public ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();
    }
}