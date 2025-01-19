using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using StatisticsJobWorker.Interfaces;
using ServiceRemoting;
using Microsoft.ServiceFabric.Data;
using Sanctuary.Models.Statistics;
using StatisticsPatientWorker.Interfaces;

namespace StatisticsJobWorker
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Volatile)]
    internal class StatisticsJobWorker : Actor, IStatisticsJobWorker, IRemindable
    {
        private readonly IServiceRemotingFactory _serviceRemotingFactory;
        private IActorReminder _reminder;
        public const string StatisticsJobKey = nameof(StatisticsJobKey);

        public const string StatsJobStatusKey = nameof(StatsJobStatusKey);
        public const string ReminderName = nameof(ReminderName);

        /// <summary>
        /// Initializes a new instance of StatisticsJobWorker
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public StatisticsJobWorker(ActorService actorService, ActorId actorId, IServiceRemotingFactory serviceRemotingFactory)
            : base(actorService, actorId)
        {
            _serviceRemotingFactory = serviceRemotingFactory;
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

        public async Task InitStatisticsJobWorker(StatisticsJobProcessingDto statisticsJob)
        {
            // setup reminder //
            _reminder = await this.RegisterReminderAsync(
                ReminderName,
                null,
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10));

            // setup / update new processing state // 
            await this.StateManager.AddOrUpdateStateAsync(StatsJobStatusKey, StatisticsJobStatus.Pending, (key, value) => StatisticsJobStatus.Pending);
            await this.StateManager.AddOrUpdateStateAsync(StatisticsJobKey, statisticsJob, (key, value) => statisticsJob);
        }

        public async Task ReceiveReminderAsync(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            var processingState = await this.StateManager.TryGetStateAsync<StatisticsJobStatus>(StatsJobStatusKey);
            if (reminderName.Equals(ReminderName)
                && processingState.HasValue
                && processingState.Value == StatisticsJobStatus.Pending)
            {

                var processingRequest = new ConditionalValue<StatisticsJobProcessingDto>(false, null);
                // update the status of execution // 
                await this.StateManager.SetStateAsync(StatsJobStatusKey, StatisticsJobStatus.Processing);
                processingRequest = await this.StateManager.TryGetStateAsync<StatisticsJobProcessingDto>(StatisticsJobKey);
                
                var patientTasks = new List<Task>();
                foreach (var patient in processingRequest.Value.PatientIds) 
                {
                    var patientActor = await _serviceRemotingFactory.GetActorServiceAsync<IStatisticsPatientWorker>(patient.Id);
                    patientTasks.Add(patientActor.ProcessPatientJob(patient, processingRequest.Value.DataFiles, processingRequest.Value.Endpoints, processingRequest.Value.StatsJobType));
                }

                await Task.WhenAll(patientTasks);


                // remove reminder if exists to allow garbage collection
                await RemoveReminder();
            }
        }

        public async Task RemoveReminder()
        {
            if (_reminder != null)
                await UnregisterReminderAsync(_reminder);
        }

    }
}
