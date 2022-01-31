using Microsoft.EntityFrameworkCore;

namespace iot_visby_weather.Data
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
