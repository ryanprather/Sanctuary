using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRemoting
{
    public class ServiceRemotingFactory : IServiceRemotingFactory
    {
        private readonly Uri _fabricUri;
        public ServiceRemotingFactory(Uri fabricUri = null)
        {
            _fabricUri = fabricUri == null ? new Uri(FabricRuntime.GetActivationContext().ApplicationName) : fabricUri;
        }

        /// <summary>
        /// gets stateless service based on the interface name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<T> GetStatelessServiceAsync<T>()
        {
            var fabricServices = await GetServiceList(ServiceKind.Stateless);

            if (!fabricServices.ContainsKey(typeof(T).Name))
                throw new KeyNotFoundException("service not found");

            var serviceProxy = new ServiceProxyFactory(c => new FabricTransportServiceRemotingClientFactory());
            fabricServices.TryGetValue(typeof(T).Name, out var serviceItem);

            return serviceProxy.CreateNonIServiceProxy<T>(serviceItem?.ServiceUri);
        }

        /// <summary>
        /// gets stateful service based on the interface name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<T> GetStatefulServiceAsync<T>(ServicePartitionKey partitionKey = null)
        {
            var fabricServices = await GetServiceList(ServiceKind.Stateful);

            if (!fabricServices.ContainsKey(typeof(T).Name))
                throw new KeyNotFoundException("service not found");

            // default to partition 0 
            partitionKey = partitionKey ?? new ServicePartitionKey(0);

            var serviceProxy = new ServiceProxyFactory(c => new FabricTransportServiceRemotingClientFactory());
            fabricServices.TryGetValue(typeof(T).Name, out var serviceItem);
            return serviceProxy.CreateNonIServiceProxy<T>(serviceItem?.ServiceUri, partitionKey);
        }

        /// <summary>
        /// gets actor service for long value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actorInstanceId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<T> GetActorServiceAsync<T>(long actorInstanceId) where T : IActor
        {
            var fabricServices = await GetServiceList(ServiceKind.Stateful);

            if (!fabricServices.ContainsKey($"{typeof(T).Name}ActorService"))
                throw new KeyNotFoundException("service not found");

            var actorServiceProxy = new ActorProxyFactory();
            return actorServiceProxy.CreateActorProxy<T>(new ActorId(actorInstanceId));
        }

        public async Task<T> GetActorServiceAsync<T>(Guid actorInstanceId) where T : IActor 
        {
            var fabricServices = await GetServiceList(ServiceKind.Stateful);

            if (!fabricServices.ContainsKey($"{typeof(T).Name}ActorService"))
                throw new KeyNotFoundException("service not found");

            var actorServiceProxy = new ActorProxyFactory();
            return actorServiceProxy.CreateActorProxy<T>(new ActorId(actorInstanceId));
        }

        /// <summary>
        /// gets list of services based on type in the cluster
        /// </summary>
        /// <returns></returns>
        private async Task<Dictionary<string, ServiceListItem>> GetServiceList(ServiceKind serviceType)
        {
            var servicesList = new Dictionary<string, ServiceListItem>();
            using (var fabricClient = new FabricClient())
            {
                var serviceFabricServices = await fabricClient.QueryManager.GetServiceListAsync(_fabricUri);
                using (var serviceEnumerator = serviceFabricServices.GetEnumerator())
                {
                    while (serviceEnumerator.MoveNext())
                    {
                        var item = serviceEnumerator.Current;
                        if (item != null && item.ServiceKind == serviceType)
                        {
                            var serviceListItem = new ServiceListItem(item.ServiceName, $"I{item.ServiceName.Segments[item.ServiceName.Segments.Length - 1]}");
                            servicesList.Add(serviceListItem.ServiceInterfaceName, serviceListItem);
                        }
                    }
                }
            }

            return servicesList;
        }

        /// <summary>
        /// internal class to hold service fabric services
        /// </summary>
        private sealed class ServiceListItem
        {
            public ServiceListItem(Uri serviceUri, string serviceInterfaceName)
            {
                ServiceUri = serviceUri;
                ServiceInterfaceName = serviceInterfaceName;
            }

            public readonly string ServiceInterfaceName;
            public readonly Uri ServiceUri;
        }

    }
}
