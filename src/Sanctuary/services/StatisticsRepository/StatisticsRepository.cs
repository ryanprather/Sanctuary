using Microsoft.IdentityModel.Tokens;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Newtonsoft.Json;
using Sanctuary.Maps;
using Sanctuary.Models.Statistics;
using Sanctuary.Statistics.Repository.Models;
using Sanctuary.Statistics.Repository.Repository;
using StatisticsRepository.Interfaces;
using System.Fabric;

namespace StatisticsRepository
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class StatisticsRepository : StatelessService, IStatisticsRepository
    {
        private readonly IStatisticsDataRepository _statisticsDataContext;
        public StatisticsRepository(StatelessServiceContext context, IStatisticsDataRepository dataContext)
            : base(context)
        {
            _statisticsDataContext = dataContext;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener((c) => new FabricTransportServiceRemotingListener(c, this)) };
        }

        public async Task<StatisticsJobDto> CreateStatisticsJobAsync(string description, StatisticsJobOptionsDto options)
        {

            var entityJob = await _statisticsDataContext.AddStatisticsJob(description, options);

            return new StatisticsJobDto()
            {
                Id = entityJob.Id,
                Description = entityJob.Description,
                Completed = entityJob.Completed,
                Created = entityJob.Created,
                JobStatus = StatisticsJobStatus.Pending,
                Options = JsonConvert.DeserializeObject<StatisticsJobOptionsDto>(entityJob.StatisticsJobDetailsJson)
            };
        }

        public async Task BatchAddStatisticsResults(Guid statisicsJobId, StatisticsResultDto[] results)
        {
            await _statisticsDataContext.AddStatisticsJobResults(statisicsJobId, results);
        }

        public async Task UpdateStartedDateForStatisticsJob(Guid id)
        {
            await _statisticsDataContext.UpdateStatisticsJobStartDate(id);
        }

        public async Task UpdateCompletedDateForStatisticsJob(Guid id)
        {
            await _statisticsDataContext.UpdateStatisticsJobCompletedDate(id);
        }

        public async Task<StatisticsJobDto[]> GetPreviousJobs()
        {
            var result = await _statisticsDataContext.GetPreviousJobs();
            return result.Select(x => new StatisticsJobDto
            {
                Id = x.Id,
                Description = x.Description,
                Completed = x.Completed,
                Created = x.Created,
                JobStatus = x.Status.ToEnum<StatisticsJobStatus>(StatisticsJobStatus.Errored),
                Options = JsonConvert.DeserializeObject<StatisticsJobOptionsDto>(x.StatisticsJobDetailsJson),
            }).ToArray();
        }

        public async Task<StatisticsJobDto> GetJobById(Guid Id)
        {
            var result = await _statisticsDataContext.QueryJobAsync(new QueryJobOptions() { JobId = Id, IncludeResults = true });
            if (result.IsSuccess)
            {
                return new StatisticsJobDto
                {
                    Id = result.Value.Id,
                    Description = result.Value.Description,
                    Completed = result.Value.Completed,
                    Created = result.Value.Created,
                    JobStatus = result.Value.Status.ToEnum<StatisticsJobStatus>(StatisticsJobStatus.Errored),
                    Options = JsonConvert.DeserializeObject<StatisticsJobOptionsDto>(result.Value.StatisticsJobDetailsJson),
                    StatisticsResults = result.Value.StatisticalResults.Select(x => new StatisticsResultDto()
                    {
                        ChartBlobUri = (x.ChartDataUri.IsNullOrEmpty()) ? null : new Uri($"{x.ChartDataUri}"),
                        DataBlobUri = (x.CsvDataUri.IsNullOrEmpty()) ? null : new Uri($"{x.CsvDataUri}"),
                    }).ToArray(),
                };
            }

            return null;
        }
    }
}
