using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Statistics.Repository.Context
{

    internal class StatisticsContextFactory : IDesignTimeDbContextFactory<StatisticsContext>
    {
        public StatisticsContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var config = configBuilder.Build();
            var builder = new DbContextOptionsBuilder<StatisticsContext>();
            builder.UseSqlServer(connectionString: config.GetConnectionString("DesignTimeConnection"));
            return new StatisticsContext(builder.Options);
        }
    }
}
