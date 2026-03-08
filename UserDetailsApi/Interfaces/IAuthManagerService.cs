using UserDetailsApi.DTOs;
using UserDetailsApi.Models;

namespace UserDetailsApi.Interfaces
{
    public interface IAuthManagerService
    {
        Task<User?> Register(UserDto userDto);
        Task<TokenResponseDto?> Login(LoginDto apiUserloginDto);
        Task<TokenResponseDto?> RefreshToken(RefreshTokenDto request);
    }
}
