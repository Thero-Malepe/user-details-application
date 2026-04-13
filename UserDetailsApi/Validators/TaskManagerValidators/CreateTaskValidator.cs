using FluentValidation;
using UserDetailsApi.DTOs.TaskManagerDtos;

namespace UserDetailsApi.Validators.TaskManagerValidators
{
    public class CreateTaskValidator : AbstractValidator<TaskDto>
    {
        public CreateTaskValidator()
        {
            RuleFor(obj => obj.Description).NotEmpty();
            RuleFor(obj => obj.Title).NotEmpty();
            RuleFor(obj => obj.DueDate).NotEmpty();
            RuleFor(obj => obj.Status).NotEmpty().IsInEnum();
            RuleFor(obj => obj.Priority).NotEmpty().IsInEnum();
        }
    }
}
