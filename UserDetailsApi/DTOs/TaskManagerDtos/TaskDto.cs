using System.ComponentModel.DataAnnotations;
using UserDetailsApi.Enums;

namespace UserDetailsApi.DTOs.TaskManagerDtos
{
    public class TaskDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Status Status { get; set; }

        public Priority Priority { get; set; }

        public DateTime? DueDate { get; set; }

    }
}
