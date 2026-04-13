using System.ComponentModel.DataAnnotations;

namespace UserDetailsApi.DTOs.AuthDtos
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }
}
