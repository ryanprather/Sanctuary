using Microsoft.Extensions.DependencyInjection;
using Sanctuary.CsvDataReader.Service;
using Sanctuary.StatisticsCalculation.OutlierAnalysis;
using ServiceRemoting;
using StatisticsLib.MovingAverage;
using StatisticsPatientWorker.Logic;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsPatientWorker._Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, StatefulServiceContext context)
        {
            services.AddTransient<IOutlierAnalysisService>(s => new OutlierAnalysisService());
            services.AddTransient<ICsvDataBlobReader>(s => new CsvDataBlobReader());
            services.AddTransient<IMovingAverageService>(s => new MovingAverageService());
            services.AddTransient<IServiceRemotingFactory>(s => new ServiceRemotingFactory());
            services.AddTransient<IStatisticsPatientLogic, StatisticsPatientLogic>();
            return services;
        }
    }
}
