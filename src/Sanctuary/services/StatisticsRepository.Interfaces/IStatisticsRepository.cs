using FluentResults;
using Microsoft.ServiceFabric.Services.Remoting;
using Sanctuary.Models.Statistics;
using StatisticsManagement.Models;

namespace StatisticsRepository.Interfaces
{
    public interface IStatisticsRepository : IService
    {
        Task<StatisticsJobProcessingDto> CreateStatisticsJobAsync(StatisticsQueueMessage queueMessage);
        Task UpdateStartedDateForStatisticsJob(Guid id);
        Task UpdateCompletedDateForStatisticsJob(Guid id);
    }
}
