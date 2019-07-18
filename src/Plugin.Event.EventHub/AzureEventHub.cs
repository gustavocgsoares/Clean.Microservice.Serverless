using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Clean.Microservice.Serverless.Plugin.Event.Entities;
using Clean.Microservice.Serverless.Plugin.Event.EventHub.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Clean.Microservice.Serverless.Plugin.Event.EventHub
{
    /// <summary>
    /// EventHub implementation using EventHub driver.
    /// </summary>
    public class AzureEventHub : IEventStrategy
    {
        private static readonly JsonSerializerSettings JsonSettings = InitializeJsonSettings();
        private readonly EventHubClient eventHubClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureEventHub"/> class.
        /// </summary>
        /// <exception cref="ArgumentException">When <paramref name="connectionString"/> is null, empty or invalid.</exception>
        /// <exception cref="ArgumentException">When <paramref name="eventName"/>  is null, empty or invalid.</exception>
        /// <param name="connectionString">EventHub connection string with format: Endpoint=sb://namespace_DNS_Name;EntityPath=EVENT_HUB_NAME;SharedAccessKeyName=SHARED_ACCESS_KEY_NAME;SharedAccessKey=SHARED_ACCESS_KEY.</param>
        /// <param name="eventName">EventHub name to set EntityPath.</param>
        /// <param name="reliabilitySettings">Event reliability settings.</param>
        public AzureEventHub(string connectionString, string eventName, EventReliabilitySettings reliabilitySettings = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(Messages.REQUIRED_PARAMETER, nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentException(Messages.REQUIRED_PARAMETER, nameof(eventName));
            }

            var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString)
            {
                TransportType = TransportType.AmqpWebSockets,
                EntityPath = eventName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            ReliabilitySettings = reliabilitySettings;
        }

        /// <inheritdoc/>
        public EventReliabilitySettings ReliabilitySettings { get; private set; }

        /// <summary>
        /// Publish a event using EventHub with EventHub driver.
        /// </summary>
        /// <exception cref="ArgumentException">When <paramref name="eventName"/> is null or empty.</exception>
        /// <param name="eventName">Event name.</param>
        /// <param name="body">Message body.</param>
        /// <param name="customProperties">List of custom properties to send with message body.</param>
        /// <returns>Task result.</returns>
        public async Task PublishAsync(string eventName, object body, IDictionary<string, object> customProperties)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentException(Messages.REQUIRED_PARAMETER, nameof(eventName));
            }

            var eventData = CreateEvent(customProperties, body);
            await SendAsync(eventData).ConfigureAwait(false);
        }

        /// <summary>
        /// Publish a event using EventHub with EventHub driver.
        /// </summary>
        /// <exception cref="ArgumentException">When <paramref name="eventName"/> is null or empty.</exception>
        /// <param name="eventName">Event name.</param>
        /// <param name="body">Message body.</param>
        /// <param name="correlationId">Correlation id.</param>
        /// <returns>Task result.</returns>
        public async Task PublishAsync(string eventName, object body, string correlationId)
        {
            var dic = new Dictionary<string, object>
            {
                { nameof(correlationId), correlationId }
            };

            await PublishAsync(eventName, body, dic).ConfigureAwait(false);
        }

        /// <summary>
        /// Publish a event using EventHub with EventHub driver.
        /// </summary>
        /// <exception cref="ArgumentException">When <paramref name="eventName"/> is null or empty.</exception>
        /// <param name="eventName">Event name.</param>
        /// <param name="correlationId">Correlation id.</param>
        /// <returns>Task result.</returns>
        public async Task PublishAsync(string eventName, string correlationId)
        {
            var dic = new Dictionary<string, object>
            {
                { nameof(correlationId), correlationId }
            };

            await PublishAsync(eventName, null, dic).ConfigureAwait(false);
        }

        /// <summary>
        /// Send data to EventHub.
        /// </summary>
        /// <param name="eventData">Data to be sent.</param>
        /// <returns>Task result.</returns>
        protected virtual async Task SendAsync(EventData eventData)
        {
            eventHubClient.SendAsync(eventData)
                .GetAwaiter()
                .GetResult();

            await Task.CompletedTask.ConfigureAwait(false);
        }

        private static JsonSerializerSettings InitializeJsonSettings()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();

            jsonSerializerSettings.Converters = new List<JsonConverter> { new StringEnumConverter() };
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Include;
            jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
            jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            jsonSerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            return jsonSerializerSettings;
        }

        private EventData CreateEvent(IDictionary<string, object> customProperties, object body)
        {
            var jsonText = JsonConvert.SerializeObject(body, JsonSettings);
            var eventData = new EventData(Encoding.UTF8.GetBytes(jsonText));

            if (customProperties != null)
            {
                foreach (var custom in customProperties)
                {
                    eventData.Properties.Add(custom);
                }
            }

            return eventData;
        }
    }
}
