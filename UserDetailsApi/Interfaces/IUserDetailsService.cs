using UserDetailsApi.DTOs;

namespace UserDetailsApi.Interfaces
{
    public interface IUserDetailsService
    {
        Task<UserDetailsDto?> GetUserDetails(string userEmali);
    }
}
