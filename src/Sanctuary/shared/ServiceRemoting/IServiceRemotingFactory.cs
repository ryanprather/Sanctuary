using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Services.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRemoting
{
    public interface IServiceRemotingFactory
    {
        Task<T> GetStatelessServiceAsync<T>();
        Task<T> GetStatefulServiceAsync<T>(ServicePartitionKey partitionKey = null);
        Task<T> GetActorServiceAsync<T>(long actorInstanceId) where T : IActor;

        Task<T> GetActorServiceAsync<T>(Guid actorInstanceId) where T : IActor;
    }
}
