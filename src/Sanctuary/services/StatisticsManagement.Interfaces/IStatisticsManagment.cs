using Microsoft.ServiceFabric.Services.Remoting;
using StatisticsManagement.Models;

namespace StatisticsManagement.Interfaces
{
    public interface IStatisticsManagement : IService
    {
        Task EnqueueStatisticsJob(StatisticsQueueMessage statisticsJob);
    }
}
