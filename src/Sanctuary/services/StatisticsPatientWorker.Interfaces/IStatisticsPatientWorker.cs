using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using Sanctuary.Models;
using Sanctuary.Models.Statistics;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace StatisticsPatientWorker.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IStatisticsPatientWorker : IActor
    {
        Task ProcessPatientJob(Guid jobId, StatisticsPatientDto patientDto, DataFileDto[] dataFiles, DataFileEndpointDto[] dataFileEndpoints, StatisticsJobCalcuationOptionsDto statsJobType);
    }
}
