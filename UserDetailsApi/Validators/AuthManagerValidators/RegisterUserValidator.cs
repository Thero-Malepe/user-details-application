using FluentValidation;
using UserDetailsApi.Models.RequestModels.UserRequestModels;

namespace UserDetailsApi.Validators.AuthManagerValidators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequestModel>
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
