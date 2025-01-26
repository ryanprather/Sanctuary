using Microsoft.Extensions.DependencyInjection;
using ServiceRemoting;
using System.Fabric;

namespace StatisticsJobWorker._Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, StatefulServiceContext context)
        {
            services.AddTransient<IServiceRemotingFactory>(s => new ServiceRemotingFactory());
            return services;
        }
    }
}
