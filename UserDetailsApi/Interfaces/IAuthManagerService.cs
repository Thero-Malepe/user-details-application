using UserDetailsApi.DTOs.AuthDtos;
using UserDetailsApi.Models;

namespace UserDetailsApi.Interfaces
{
    public interface IAuthManagerService
    {
        Task<User?> Register(UserDto userDto);
        Task<TokenResponseDto?> Login(LoginDto apiUserloginDto);
        Task<TokenResponseDto?> RefreshToken(RefreshTokenDto request);
        Task<bool> PasswordResetEmail(string email);
        Task<bool> ResetPassword(ResetDto email);
    }
}
