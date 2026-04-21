using FluentValidation;
using UserDetailsApi.Models.RequestModels.TaskRequestModels;

namespace UserDetailsApi.Validators.TaskManagerValidators
{
    public class DeleteTaskValidator : AbstractValidator<DeleteTaskRequestModel>
    {
        public DeleteTaskValidator()
        {
            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0);
        }
    }
}
