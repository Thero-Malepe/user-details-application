using Microsoft.EntityFrameworkCore;
using UserDetailsApi.Models;

namespace UserDetailsApi.Data
{
    public class UserDetailsDbContext(DbContextOptions<UserDetailsDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    }
}
