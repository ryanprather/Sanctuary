using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Sanctuary.Models;
using Sanctuary.Models.Statistics;
using ServiceRemoting;
using StatisticsPatientWorker.Interfaces;
using StatisticsPatientWorker.Logic;
using StatisticsRepository.Interfaces;
using System.Data;

namespace StatisticsPatientWorker
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class StatisticsPatientWorker : Actor, IStatisticsPatientWorker
    {

        private readonly IServiceRemotingFactory _serviceRemotingFactory;
        private readonly IStatisticsPatientLogic _logic;
        /// <summary>
        /// Initializes a new instance of StatisticsPatientWorker
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public StatisticsPatientWorker(
            ActorService actorService, 
            ActorId actorId,
            IServiceRemotingFactory serviceRemotingFactory,
            IStatisticsPatientLogic logic) 
            : base(actorService, actorId)
        {
            _serviceRemotingFactory = serviceRemotingFactory;
            _logic = logic;
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            return Task.CompletedTask;
        }

        public async Task ProcessPatientJob(Guid jobId, StatisticsPatientDto patientDto, DataFileDto[] dataFiles, DataFileEndpointDto[] dataFileEndpoints, StatisticsJobCalcuationOptionsDto statsJobType)
        {
            var results = await _logic.ProcessRequest(patientDto, dataFiles, dataFileEndpoints, statsJobType);
            var convertedResults = results.Select(x => new StatisticsResultDto() 
            {
                ResultOptions = new StatisticsResultOptions() { PatientIdentifer = patientDto.Identifier},
                ChartBlobUri = x.ChartBlobUri,
                DataBlobUri = x.DataBlobUri,
            }).ToArray();
            
            var repositoryService = await _serviceRemotingFactory.GetStatelessServiceAsync<IStatisticsRepository>();
            await repositoryService.BatchAddStatisticsResults(jobId, convertedResults);
        }
            
    }
}
