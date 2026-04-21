using FluentValidation;
using UserDetailsApi.Models.RequestModels.UserRequestModels;

namespace UserDetailsApi.Validators.AuthManagerValidators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequestModel>
    {
        public ResetPasswordValidator()
        {
            RuleFor(obj => obj.Email).NotEmpty().EmailAddress();
            RuleFor(obj => obj.Password).NotEmpty().MinimumLength(8).MaximumLength(15);
            RuleFor(obj => obj.Token).NotEmpty();
        }
    }
}
