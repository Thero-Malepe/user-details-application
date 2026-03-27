using System.ComponentModel.DataAnnotations;

namespace UserDetailsApi.Models
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; }
    }
}
