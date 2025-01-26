using Microsoft.EntityFrameworkCore;
using Sanctuary.Statistics.Repository.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Statistics.Repository.Context
{
    public class StatisticsContext: DbContext
    {
        public DbSet<StatisticalResult> StatisticalResults { get; set; }
        public DbSet<StatisticsJob> StatisticsJobs { get; set; }
        public DbSet<StatisticsJobType> StatisticsJobTypes { get; set; }
        public DbSet<StatisticsJobPatient> StatisticsJobPatients { get; set; }
        public DbSet<StatisticsJobEndpoint> StatisticsJobEndpoints { get; set; }
        public DbSet<StatisticsJobDataFile> StatisticsJobDataFiles { get; set; }

        public StatisticsContext() { }
        public StatisticsContext(DbContextOptions options) : base(options) { }

        



    }
}
