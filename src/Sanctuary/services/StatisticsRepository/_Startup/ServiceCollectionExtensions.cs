using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sanctuary.Statistics.Repository.Context;
using Sanctuary.Statistics.Repository.Repository;
using ServiceRemoting;
using System.Fabric;

namespace StatisticsRepository._Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, StatelessServiceContext context)
        {
            var config = GetConfiguration();
            var optionsBuilder = new DbContextOptionsBuilder<StatisticsContext>();
            optionsBuilder.UseSqlServer(connectionString: config.TrailManagmentSqlConnection);
            
            services.AddTransient<IStatisticsDataRepository>(s => new StatisticsDataRepository(new StatisticsContext(optionsBuilder.Options)));
            return services;
        }

        private static Configuration GetConfiguration()
        {
            return new Configuration()
            {
                TrailManagmentSqlConnection = "Data Source=.\\SQLEXPRESS;Initial Catalog=SanctuaryDemo;TrustServerCertificate=True;Integrated Security = True;"
            };
        }

        private class Configuration
        {
            public string TrailManagmentSqlConnection { get; set; }
        }
    }
}
