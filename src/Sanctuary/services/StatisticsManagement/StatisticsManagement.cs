using System.Fabric;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Sanctuary.Models.Statistics;
using ServiceRemoting;
using StatisticsJobWorker.Interfaces;
using StatisticsManagement.Interfaces;
using StatisticsManagement.Models;
using StatisticsRepository.Interfaces;

namespace StatisticsManagement
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    public class StatisticsManagement : StatefulService, IStatisticsManagement
    {
        private const string StatisticsManagementQueueName = "statistics-management-queue";
        private readonly IServiceRemotingFactory _serviceRemotingFactory;
        private const string StatisticsManagementDictionaryName = "statistics-management-dictionary";
        public const int RunAsyncWaitTime = 2000;

        public StatisticsManagement(
            StatefulServiceContext context,
            IServiceRemotingFactory serviceRemotingFactory)
            : base(context)
        {
            _serviceRemotingFactory = serviceRemotingFactory;
        }

        public async Task EnqueueStatisticsJob(StatisticsJobDto statisticsJob)
        {
            var queue = await this.StateManager
                    .GetOrAddAsync<IReliableConcurrentQueue<StatisticsJobDto>>(StatisticsManagementQueueName);
            using var tx = this.StateManager.CreateTransaction();
            await queue.EnqueueAsync(tx, statisticsJob);
            await tx.CommitAsync();
        }

        public async Task<StatisticsJobDto?> DequeueStatisticsJob()
        {
            var queue = await this.StateManager.GetOrAddAsync<IReliableConcurrentQueue<StatisticsJobDto>>(StatisticsManagementQueueName);
            
            if (queue.Count <= 0) return default;
            using var tx = this.StateManager.CreateTransaction();
            var item = await queue.TryDequeueAsync(tx);
            await tx.CommitAsync();

            return item.Value;
        }

        public async Task AddWorkItemToDictionary(StatisticsJobDto statisticsJobProcessingDto) 
        {   
            var workItemDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, StatisticsJobDto>>(StatisticsManagementDictionaryName);
            using var tx = this.StateManager.CreateTransaction();
            try
            {
                await workItemDictionary.AddAsync(tx, statisticsJobProcessingDto.Id, statisticsJobProcessingDto);
                await tx.CommitAsync();
            }
            catch
            {
                tx.Abort();
                throw;
            }
        }

        public async Task<List<StatisticsJobDto>> GetNextWorkItemsAsync(CancellationToken cancellationToken) 
        {
            var statsJobQueueList = new List<StatisticsJobDto>();
            // Get dictionary
            var workItemDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, StatisticsJobDto>>(StatisticsManagementDictionaryName);

            // Get any available stats jobs to process //
            using (var tx = this.StateManager.CreateTransaction())
            {
                var statsJobsAsync = await workItemDictionary.CreateEnumerableAsync(tx, EnumerationMode.Ordered);
                using var statsJobsEnum = statsJobsAsync.GetAsyncEnumerator();
                
                while (await statsJobsEnum.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                {
                    if (statsJobsEnum.Current.Value.JobStatus == StatisticsJobStatus.Pending)
                    {
                        statsJobQueueList.Add(statsJobsEnum.Current.Value);
                    }
                }
            }

            return statsJobQueueList;
        }

        public async Task ProcessStatsJobs(List<StatisticsJobDto> pendingStatsJobs)
        {
            // Get dictionary
            var workItemDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, StatisticsJobDto>>(StatisticsManagementDictionaryName);
            foreach (var kvp in pendingStatsJobs)
            {
                using (var tx = this.StateManager.CreateTransaction())
                {
                    try
                    {
                        StatisticsJobDto updatedStatisticsJob;
                        var workItem = await workItemDictionary.TryGetValueAsync(tx, kvp.Id);
                        if (!workItem.HasValue)
                            continue;

                        updatedStatisticsJob = new StatisticsJobDto();
                        updatedStatisticsJob.Id = workItem.Value.Id;
                        updatedStatisticsJob.Created = workItem.Value.Created;
                        updatedStatisticsJob.JobStatus = StatisticsJobStatus.Processing;
                        updatedStatisticsJob.Options = workItem.Value.Options;
                        await workItemDictionary.SetAsync(tx, updatedStatisticsJob.Id, updatedStatisticsJob);
                        await tx.CommitAsync();

                        var statsJobWorker = await _serviceRemotingFactory.GetActorServiceAsync<IStatisticsJobWorker>(updatedStatisticsJob.Id);
                        await statsJobWorker.InitStatisticsJobWorker(updatedStatisticsJob);
                    }
                    catch (Exception ex)
                    {
                        tx.Abort();
                    }
                }
            }
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context=>
                    new FabricTransportServiceRemotingListener(
                        context,
                        this,
                        new FabricTransportRemotingListenerSettings()
                        {
                            ExceptionSerializationTechnique = FabricTransportRemotingListenerSettings.ExceptionSerialization.Default
                        })),
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            
            ManualResetEvent RunAsyncWait = new ManualResetEvent(false);

            while (!cancellationToken.IsCancellationRequested)
            {
                var queuedItem = await DequeueStatisticsJob();
                var workItems = await GetNextWorkItemsAsync(cancellationToken);
                
                if (queuedItem is not null)
                    await AddWorkItemToDictionary(queuedItem);

                if(workItems.Any())
                    await ProcessStatsJobs(workItems);


                RunAsyncWait.WaitOne(RunAsyncWaitTime, false); // Same as Thread.Sleep(1000);
            }
        }

    }
}
