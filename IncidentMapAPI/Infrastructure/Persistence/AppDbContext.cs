using IncidentMapAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentMapAPI.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>opt) : base(opt)
        {
            
        }

        public DbSet<Incident>IncidentTable { get; set; }
    }
}
