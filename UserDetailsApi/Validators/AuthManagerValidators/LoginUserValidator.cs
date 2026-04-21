using FluentValidation;
using UserDetailsApi.Models.RequestModels.UserRequestModels;

namespace UserDetailsApi.Validators.AuthManagerValidators
{
    public class LoginUserValidator : AbstractValidator<LoginUserRequestModel>
    {
        public LoginUserValidator()
        {
            RuleFor(obj => obj.Email).NotEmpty().EmailAddress();
            RuleFor(obj => obj.Password).NotEmpty().MaximumLength(15).MinimumLength(8);
        }
    }
}
