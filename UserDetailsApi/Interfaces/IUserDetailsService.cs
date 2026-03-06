using UserDetailsApi.DTOs;
using UserDetailsApi.Models;

namespace UserDetailsApi.Interfaces
{
    public interface IUserDetailsService
    {
        Task<UserDetailsDto?> GetUserDetails(string userDto);
        Task<TokenResponseDto?> UpdateUserDetails(LoginDto apiUserloginDto);
    }
}
