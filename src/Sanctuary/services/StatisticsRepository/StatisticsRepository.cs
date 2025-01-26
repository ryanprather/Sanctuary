using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Sanctuary.Models.Statistics;
using Sanctuary.Statistics.Repository.Repository;
using StatisticsManagement.Models;
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

        public async Task<StatisticsJobProcessingDto> CreateStatisticsJobAsync(StatisticsQueueMessage queueMessage) 
        {
            
            var entityJob = await _statisticsDataContext.AddStatisticsJob(queueMessage.Description, 
                queueMessage.DataFiles, queueMessage.Endpoints, queueMessage.Patients, 
                queueMessage.StatsJobType);
            
            return new StatisticsJobProcessingDto() 
            {
                Id = entityJob.Id,
                Description = entityJob.Description,
                Completed = entityJob.Completed,
                Created = entityJob.Created,
                JobStatus = StatisticsJobStatus.Pending,
                StatsJobType = queueMessage.StatsJobType,
                DataFiles = queueMessage.DataFiles,
                Endpoints = queueMessage.Endpoints,
                PatientIds = queueMessage.Patients    
            };
        }

        public async Task UpdateStartedDateForStatisticsJob(Guid id) 
        {
            await _statisticsDataContext.UpdateStatisticsJobStartDate(id);
        }

        public async Task UpdateCompletedDateForStatisticsJob(Guid id)
        {
            await _statisticsDataContext.UpdateStatisticsJobCompletedDate(id);
        }
    }
}
