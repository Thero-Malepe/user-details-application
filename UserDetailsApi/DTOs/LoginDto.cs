using System.ComponentModel.DataAnnotations;

namespace UserDetailsApi.DTOs
{
    public class LoginDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(maximumLength: 15, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
