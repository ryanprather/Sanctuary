using Microsoft.EntityFrameworkCore;
using Sanctuary.Statistics.Repository.Datasets;

namespace Sanctuary.Statistics.Repository.Context
{
    public class StatisticsContext: DbContext
    {
        public DbSet<StatisticalResult> StatisticalResults { get; set; }
        public DbSet<StatisticsJob> StatisticsJobs { get; set; }
      
        public StatisticsContext() { }
        public StatisticsContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatisticsJob>().HasMany(s => s.StatisticalResults).WithOne(s => s.StatisticsJob);
        }

    }
}
