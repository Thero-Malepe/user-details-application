using FluentValidation;
using UserDetailsApi.Models.RequestModels.TaskRequestModels;

namespace UserDetailsApi.Validators.TaskManagerValidators
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskRequestModel>
    {
        public UpdateTaskValidator()
        {
            RuleFor(obj => obj.Description).NotEmpty();
            RuleFor(obj => obj.Title).NotEmpty();
            RuleFor(obj => obj.DueDate).NotEmpty();
            RuleFor(obj => obj.Status).NotEmpty().IsInEnum();
            RuleFor(obj => obj.Priority).NotEmpty().IsInEnum();
        }
    }
}
