using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Actors.Runtime;
using ServiceRemoting;
using System.Fabric;

namespace StatisticsJobWorker._Startup
{
    internal static class StatisticsJobWorkerFactory
    {
        internal static ActorService Create(StatefulServiceContext context, ActorTypeInformation actorType)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCustomServices(context);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return new ActorService(context, actorType, (service, id) =>
                new StatisticsJobWorker(
                    service,
                    id,
                    serviceProvider.GetRequiredService<IServiceRemotingFactory>()));
        }
    }

}
