using Microsoft.EntityFrameworkCore;

namespace simple_iot_weather_station.Data
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
