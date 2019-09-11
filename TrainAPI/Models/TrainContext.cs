using Microsoft.EntityFrameworkCore;

namespace TrainAPI.Models
{
    public class TrainContext : DbContext
    {
        public TrainContext(DbContextOptions<TrainContext> options) : base(options)
        {

        }

        public TrainContext()
        {

        }

        public DbSet<Train> Trains { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Station> Stations { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
