using FluentValidation;
using UserDetailsApi.DTOs.AuthDtos;
using UserDetailsApi.DTOs.TaskManagerDtos;

namespace UserDetailsApi.Validators.AuthManagerValidators
{
    public class ResetPasswordValidator : AbstractValidator<ResetDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(obj => obj.Email).NotEmpty().EmailAddress();
            RuleFor(obj => obj.Password).NotEmpty().MinimumLength(8).MaximumLength(15);
            RuleFor(obj => obj.Token).NotEmpty();
        }
    }
}
