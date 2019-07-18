using System.Collections.Generic;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.Plugin.Event.Entities;

namespace Clean.Microservice.Serverless.Plugin.Event
{
    /// <summary>
    /// Interface to event publisher.
    /// </summary>
    public interface IEventStrategy
    {
        /// <summary>
        /// Gets reliability settings if necessary.
        /// </summary>
        /// <value>
        /// Reliability settings if necessary.
        /// </value>
        EventReliabilitySettings ReliabilitySettings { get; }

        /// <summary>
        /// Publish a event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="body">Message body.</param>
        /// <param name="correlationId">Correlation id.</param>
        /// <returns>Task result.</returns>
        Task PublishAsync(string eventName, object body, string correlationId);

        /// <summary>
        /// Publish a event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="correlationId">Correlation id.</param>
        /// <returns>Task result.</returns>
        Task PublishAsync(string eventName, string correlationId);

        /// <summary>
        /// Publish a event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="body">Message body.</param>
        /// <param name="customProperties">List of custom properties to send with message body.</param>
        /// <returns>Task result.</returns>
        Task PublishAsync(string eventName, object body, IDictionary<string, object> customProperties);
    }
}
