using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceRemoting;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsManagement._Startup
{
    internal static class StatisticsManagementFactory
    {
        internal static StatisticsManagement Create(StatefulServiceContext context)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCustomServices(context);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return new StatisticsManagement(context,
                serviceProvider.GetRequiredService<IServiceRemotingFactory>());
        }
    }
}
