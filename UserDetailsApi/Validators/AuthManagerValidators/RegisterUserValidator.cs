using FluentValidation;
using UserDetailsApi.DTOs.AuthDtos;
using UserDetailsApi.DTOs.TaskManagerDtos;

namespace UserDetailsApi.Validators.AuthManagerValidators
{
    public class RegisterUserValidator : AbstractValidator<UserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(obj => obj.Email).NotEmpty().EmailAddress();
            RuleFor(obj => obj.Password).NotEmpty().MaximumLength(15).MinimumLength(8);
            RuleFor(obj => obj.FirstName).NotEmpty();
            RuleFor(obj => obj.LastName).NotEmpty();
        }
    }
}
