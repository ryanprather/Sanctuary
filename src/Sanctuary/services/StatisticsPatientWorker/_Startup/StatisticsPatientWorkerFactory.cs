using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Actors.Runtime;
using Sanctuary.CsvDataReader.Service;
using Sanctuary.StatisticsCalculation.OutlierAnalysis;
using ServiceRemoting;
using StatisticsLib.MovingAverage;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    serviceProvider.GetRequiredService<IMovingAverageService>(),
                    serviceProvider.GetRequiredService<IOutlierAnalysisService>(),
                    serviceProvider.GetRequiredService<ICsvDataBlobReader>()));
        }
    }

}
