using FluentValidation;

namespace UserDetailsApi.Validators.TaskManagerValidators
{
    public class GetTaskByIdValidator : AbstractValidator<int>
    {
        public GetTaskByIdValidator()
        {
            RuleFor(obj => obj).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
