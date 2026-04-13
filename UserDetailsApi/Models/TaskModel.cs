using System.ComponentModel.DataAnnotations;
using UserDetailsApi.Enums;

namespace UserDetailsApi.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required]
        public Status Status { get; set; }

        [Required]
        public Priority Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = new DateTime();

    }
}
