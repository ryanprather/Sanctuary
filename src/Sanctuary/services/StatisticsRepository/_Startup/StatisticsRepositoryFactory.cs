using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Abstractions;
using Sanctuary.Statistics.Repository.Repository;
using ServiceRemoting;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsRepository._Startup
{
    internal class StatisticsRepositoryFactory
    {
        internal static StatisticsRepository Create(StatelessServiceContext context)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCustomServices(context);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return new StatisticsRepository(context,
                serviceProvider.GetRequiredService<IStatisticsDataRepository>());
        }
    }
}
