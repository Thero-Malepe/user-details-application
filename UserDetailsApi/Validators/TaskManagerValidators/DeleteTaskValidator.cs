using FluentValidation;

namespace UserDetailsApi.Validators.TaskManagerValidators
{
    public class DeleteTaskValidator : AbstractValidator<int>
    {
        public DeleteTaskValidator()
        {
            RuleFor(obj => obj).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
