using FluentValidation;
using UserDetailsApi.DTOs.AuthDtos;
using UserDetailsApi.DTOs.TaskManagerDtos;

namespace UserDetailsApi.Validators.AuthManagerValidators
{
    public class LoginUserValidator : AbstractValidator<LoginDto>
    {
        public LoginUserValidator()
        {
            RuleFor(obj => obj.Email).NotEmpty().EmailAddress();
            RuleFor(obj => obj.Password).NotEmpty().MaximumLength(15).MinimumLength(8);
        }
    }
}
