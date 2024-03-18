using Message.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Message.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserMessage> Messages { get; set; }
    }
}
