using System.ComponentModel.DataAnnotations;

namespace UserDetailsApi.DTOs.AuthDtos
{
    public class UserDetailsDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
