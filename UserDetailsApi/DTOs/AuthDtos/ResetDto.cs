using System.ComponentModel.DataAnnotations;

namespace UserDetailsApi.DTOs.AuthDtos
{
    public class ResetDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
