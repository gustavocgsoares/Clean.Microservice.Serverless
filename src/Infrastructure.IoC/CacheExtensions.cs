using System;
using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.Core.Constants;
using Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC;

namespace Clean.Microservice.Serverless.Infrastructure.IoC
{
    internal static class CacheExtensions
    {
        public static void AddCache(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable(EnvironmentConstants.ConnectionStringCache);
            var instanceName = string.Concat(ApplicationConstants.ApplicationName, ":");

            services.AddRedisCache(connectionString, instanceName);
        }
    }
}
