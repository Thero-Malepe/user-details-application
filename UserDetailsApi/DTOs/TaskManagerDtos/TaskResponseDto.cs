using System.ComponentModel.DataAnnotations;
using UserDetailsApi.Enums;

namespace UserDetailsApi.DTOs.TaskManagerDtos
{
    public class TaskResponseDto
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public Status Status { get; set; }

        [Required]
        public Priority Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
