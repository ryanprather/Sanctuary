﻿using Microsoft.Extensions.DependencyInjection;
using ServiceRemoting;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsManagement._Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, StatefulServiceContext context)
        {
            //var configValues = GetConfiguration();
            services.AddTransient<IServiceRemotingFactory>(s => new ServiceRemotingFactory());
            
            return services;
        }
    }
}