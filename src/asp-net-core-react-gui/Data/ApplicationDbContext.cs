using Microsoft.EntityFrameworkCore;

namespace frontend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //EMPTY
        }
        public DbSet<Models.WeatherReport> WeatherReports { get; set; }
    }
}
