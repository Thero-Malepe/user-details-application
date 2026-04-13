using UserDetailsApi.DTOs.AuthDtos;

namespace UserDetailsApi.Interfaces
{
    public interface IUserDetailsService
    {
        Task<UserDetailsDto?> GetUserDetails(string userEmali);
    }
}
