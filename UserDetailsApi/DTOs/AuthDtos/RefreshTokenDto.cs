using System.ComponentModel.DataAnnotations;

namespace UserDetailsApi.DTOs.AuthDtos
{
    public class RefreshTokenDto
    {
        public string AccessToken{ get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
