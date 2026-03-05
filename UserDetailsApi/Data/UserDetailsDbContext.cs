using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using UserDetailsApi.Models;

namespace UserDetailsApi.Data
{
    public class UserDetailsDbContext(DbContextOptions<UserDetailsDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }        
    }
}
