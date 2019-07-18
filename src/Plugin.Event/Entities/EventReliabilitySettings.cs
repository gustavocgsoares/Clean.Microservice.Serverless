using System;

namespace Clean.Microservice.Serverless.Plugin.Event.Entities
{
    /// <summary>
    /// Settings for event reliability to retry poisoned event.
    /// </summary>
    public class EventReliabilitySettings
    {
        /// <summary>
        /// Gets the connection string for republish poisoned event.
        /// </summary>
        /// <value>
        /// Connection string for republish poisoned event.
        /// </value>
        public virtual string EventPublisherConnectionString { get; private set; }

        /// <summary>
        /// Gets the maximum of retry attempts before publish the poisoned event.
        /// </summary>
        /// <value>
        /// Maximum of retry attempts before publish the poisoned event.
        /// </value>
        public virtual uint RetryAttempts { get; private set; }

        /// <summary>
        /// Gets the sleep duration to retry.
        /// </summary>
        /// <value>
        /// The sleep duration to retry.
        /// </value>
        public virtual TimeSpan RetrySleepDuration { get; private set; }

        /// <summary>
        /// Builder for event reliability settings.
        /// </summary>
        /// <param name="eventPublisherConnectionString">Connection string for republish poisoned event.</param>
        /// <param name="retryAttempts">Maximum of retry attempts before publish the poisoned event.</param>
        /// <param name="retrySleepDuration">The sleep duration to retry.</param>
        /// <returns>Event reliability settings</returns>
        public static EventReliabilitySettings Builder(
            string eventPublisherConnectionString,
            uint retryAttempts,
            TimeSpan retrySleepDuration)
        {
            if (string.IsNullOrWhiteSpace(eventPublisherConnectionString))
            {
                throw new ArgumentNullException(nameof(eventPublisherConnectionString));
            }

            return new EventReliabilitySettings
            {
                EventPublisherConnectionString = eventPublisherConnectionString,
                RetryAttempts = retryAttempts,
                RetrySleepDuration = retrySleepDuration
            };
        }
    }
}
