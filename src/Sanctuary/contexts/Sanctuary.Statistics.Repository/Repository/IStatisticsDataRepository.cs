using FluentResults;
using Sanctuary.Models.Statistics;
using Sanctuary.Statistics.Repository.Datasets;
using Sanctuary.Statistics.Repository.Models;

namespace Sanctuary.Statistics.Repository.Repository
{
    public interface IStatisticsDataRepository
    {
        Task<StatisticsJob> AddStatisticsJob(string description, StatisticsJobOptionsDto options);
        Task UpdateStatisticsJobStartDate(Guid jobId);
        Task UpdateStatisticsJobCompletedDate(Guid jobId);
        Task AddStatisticsJobResults(Guid jobId, StatisticsResultDto[] statisticsResultDto);
        Task<IList<StatisticsJob>> GetPreviousJobs();
        Task<Result<StatisticsJob>> QueryJobAsync(QueryJobOptions options);

    }
}
