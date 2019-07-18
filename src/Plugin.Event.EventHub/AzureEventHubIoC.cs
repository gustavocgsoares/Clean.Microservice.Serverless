using System;
using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.Plugin.Event.Entities;

namespace Clean.Microservice.Serverless.Plugin.Event.EventHub
{
    /// <summary>
    /// Class to manage EventHub with driver using dependency injection.
    /// </summary>
    public static class AzureEventHubIoC
    {
        /// <summary>
        /// Add EventHub publisher with IoC.
        /// </summary>
        /// <exception cref="ArgumentException">When <paramref name="eventName"/> or <paramref name="connectionString"/> is null, empty or invalid.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString"/> is invalid.</exception>
        /// <param name="services"><see cref="IServiceCollection"/> injection.</param>
        /// <param name="eventName">EventHub name.</param>
        /// <param name="connectionString">EventHub connection string with format: Endpoint=sb://namespace_DNS_Name;SharedAccessKeyName=SHARED_ACCESS_KEY_NAME;SharedAccessKey=SHARED_ACCESS_KEY;EntityPath=.</param>
        /// <param name="reliabilitySettings">Event reliability settings.</param>
        /// <returns><see cref="IServiceCollection"/> with EventHub plugin injected.</returns>
        public static IServiceCollection AddEventHubPlugin(
            this IServiceCollection services,
            string eventName,
            string connectionString,
            EventReliabilitySettings reliabilitySettings = null)
        {
            services.AddEventPlugin(eventName, new AzureEventHub(connectionString, eventName, reliabilitySettings));
            return services;
        }
    }
}
