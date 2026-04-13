using FluentValidation;
using UserDetailsApi.Models;

namespace UserDetailsApi.Validators.TaskManagerValidators
{
    public class UpdateTaskValidator : AbstractValidator<TaskModel>
    {
        public UpdateTaskValidator()
        {
            RuleFor(obj => obj.Id).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(obj => obj.Description).NotEmpty();
            RuleFor(obj => obj.Title).NotEmpty();
            RuleFor(obj => obj.DueDate).NotEmpty();
            RuleFor(obj => obj.Status).NotEmpty().IsInEnum();
            RuleFor(obj => obj.Priority).NotEmpty().IsInEnum();
        }
    }
}
