using FluentValidation;
using UserDetailsApi.DTOs.AuthDtos;

namespace UserDetailsApi.Validators.AuthManagerValidators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenValidator()
        {
            RuleFor(obj => obj.RefreshToken).NotEmpty();
            RuleFor(obj => obj.AccessToken).NotEmpty();
        }        
    }
}
