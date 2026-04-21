using UserDetailsApi.Enums;

namespace UserDetailsApi.Models.RequestModels.TaskRequestModels
{
    public class CreateTaskRequestModel
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Status Status { get; set; }

        public Priority Priority { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
