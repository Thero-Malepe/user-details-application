using Microsoft.EntityFrameworkCore;
using UserDetailsApi.Data;
using UserDetailsApi.DTOs.AuthDtos;
using UserDetailsApi.Interfaces;
using UserDetailsApi.Models;

namespace UserDetailsApi.Services
{
    public class UserDetailsService(UserDetailsDbContext context, ILogger<TaskManagereService> logger) : IUserDetailsService
    {
        public async Task<UserDetailsDto?> GetUserDetails(string userId)
        {
            logger.LogInformation("Retrieving details for user :{id}", userId);
            if (Guid.TryParse(userId, out Guid id))
            {
                var user = await context.Users.FindAsync(id);
                if (user is null)
                {
                    logger.LogError("User with Id: {id} not found", id);
                    return null;
                }

                var userDetailsDto = new UserDetailsDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };

                logger.LogInformation("Retrieved details for user :{id}", id);
                return userDetailsDto;
            }

            logger.LogError("Invalid Id: {id}", userId);
            return null;
        }
    }
}
