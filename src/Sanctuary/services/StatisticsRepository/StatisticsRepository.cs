using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Sanctuary.Models.Statistics;
using StatisticsManagement.Models;
using StatisticsRepository.Interfaces;

namespace StatisticsRepository
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class StatisticsRepository : StatelessService, IStatisticsRepository
    {
        public StatisticsRepository(StatelessServiceContext context)
            : base(context)
        { }

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
            var job = new StatisticsJobProcessingDto() 
            {
                Id = Guid.NewGuid(),
                JobStatus = StatisticsJobStatus.Pending,
                Created = DateTime.UtcNow,
                DataFiles = queueMessage.DataFiles,
                Description = queueMessage.Description,
                PatientIds = queueMessage.Patients,
                Endpoints = queueMessage.Endpoints,
                StatsJobType = queueMessage.StatsJobType,
            };
            return job;
        }
    }
}
