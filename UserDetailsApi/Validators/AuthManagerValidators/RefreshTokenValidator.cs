using FluentValidation;
using UserDetailsApi.Models.RequestModels.UserRequestModels;

namespace UserDetailsApi.Validators.AuthManagerValidators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequestModel>
    {
        public RefreshTokenValidator()
        {
            RuleFor(obj => obj.RefreshToken).NotEmpty();
            RuleFor(obj => obj.AccessToken).NotEmpty();
        }        
    }
}
