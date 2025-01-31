using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Actors.Runtime;
using ServiceRemoting;
using StatisticsPatientWorker.Logic;
using System.Fabric;

namespace StatisticsPatientWorker._Startup
{
    internal static class StatisticsPatientWorkerFactory
    {
        internal static ActorService Create(StatefulServiceContext context, ActorTypeInformation actorType)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCustomServices(context);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return new ActorService(context, actorType, (service, id) =>
                new StatisticsPatientWorker(
                    service,
                    id,
                    serviceProvider.GetRequiredService<IServiceRemotingFactory>(),
                    serviceProvider.GetRequiredService<IStatisticsPatientLogic>()));
        }
    }

}
