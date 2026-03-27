using System.ComponentModel.DataAnnotations;

namespace UserDetailsApi.DTOs
{
    public class ResetDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(maximumLength: 15, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
