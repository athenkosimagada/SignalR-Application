using Microsoft.EntityFrameworkCore;
using SignalR.Server.Models;

namespace SignalR.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Connection> Connections { get; set; }
    }
}
