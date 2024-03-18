using Microsoft.EntityFrameworkCore;
using Post.Server.Models;

namespace Post.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserPost> Posts { get; set; }
    }
}
