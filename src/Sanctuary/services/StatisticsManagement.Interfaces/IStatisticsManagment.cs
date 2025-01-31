using Microsoft.ServiceFabric.Services.Remoting;
using Sanctuary.Models.Statistics;
using StatisticsManagement.Models;

namespace StatisticsManagement.Interfaces
{
    public interface IStatisticsManagement : IService
    {
        Task EnqueueStatisticsJob(StatisticsJobDto statisticsJob);
    }
}
