using Microsoft.ServiceFabric.Services.Remoting;
using Sanctuary.Models.Statistics;

namespace StatisticsRepository.Interfaces
{
    public interface IStatisticsRepository : IService
    {
        Task<StatisticsJobDto> CreateStatisticsJobAsync(string description, StatisticsJobOptionsDto options); 
        Task UpdateStartedDateForStatisticsJob(Guid id);
        Task UpdateCompletedDateForStatisticsJob(Guid id);
        Task BatchAddStatisticsResults(Guid statisicsJobId, StatisticsResultDto[] results);
    }
}
