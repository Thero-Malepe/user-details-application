using Microsoft.EntityFrameworkCore;
using UserDetailsApi.Data;
using UserDetailsApi.DTOs;
using UserDetailsApi.Interfaces;

namespace UserDetailsApi.Services
{
    public class UserDetailsService(UserDetailsDbContext context) : IUserDetailsService
    {
        public async Task<UserDetailsDto?> GetUserDetails(string userEmail)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userEmail.ToLower());

            if (user is null)
            {
                return null;
            }
            var userDetailsDto = new UserDetailsDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            
            return userDetailsDto;
        }
    }
}
