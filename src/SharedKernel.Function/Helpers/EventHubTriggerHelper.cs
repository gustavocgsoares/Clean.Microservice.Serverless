using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Logging;
using Clean.Microservice.Serverless.Plugin.Event.Entities;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Results;
using Polly;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Helpers
{
    /// <summary>
    /// Helpers for event hub trigger.
    /// </summary>
    public static class EventHubTriggerHelper
    {
        /// <summary>
        /// Create pattern log.
        /// </summary>
        /// <param name="className">Name of trigger class.</param>
        /// <returns>Pattern log created.</returns>
        public static string CreatePatternLog(string className)
        {
            return $"[{Guid.NewGuid()} | {className}] ";
        }

        /// <summary>
        /// Extract correlationId value from <see cref="EventData"/> properties.
        /// </summary>
        /// <param name="eventData">Event data.</param>
        /// <param name="serverlessLogger">Logger to save log with correlationId extracted.</param>
        /// <param name="patternLog">Pattern log.</param>
        /// <returns>CorrelationId value extracted.</returns>
        public static string GetCorrelationId(EventData eventData, IServerlessLogger serverlessLogger, string patternLog)
        {
            var properties = eventData.Properties;

            properties.TryGetValue("correlationId", out var correlationId);

            serverlessLogger.LogInformation($"{patternLog} Processing event with {nameof(correlationId)}: {correlationId}!");

            return (correlationId ?? Guid.NewGuid()).ToString();
        }

        /// <summary>
        /// Start event hub process.
        /// </summary>
        /// <param name="serverlessLogger">Logger to save log.</param>
        /// <param name="logger">Logger to be injected in <paramref name="serverlessLogger"/>.</param>
        /// <param name="patternLog">Pattern log.</param>
        /// <param name="events">Events to be processed.</param>
        /// <param name="processEventAsync">Process with business rules to be processed.</param>
        /// <param name="publishPoisonedEventAsync">Function receiving error parameter to publish poisoned event data.</param>
        /// <param name="eventReliabilitySettings">Event reliability settings.</param>
        /// <returns>Task executed.</returns>
        public static async Task StartEventProcessAsync(
            IServerlessLogger serverlessLogger,
            ILogger logger,
            string patternLog,
            EventData[] events,
            Func<EventData, Task> processEventAsync,
            Func<EventData, Exception, Task> publishPoisonedEventAsync,
            EventReliabilitySettings eventReliabilitySettings)
        {
            try
            {
                serverlessLogger.InjectLogger(logger);

                if (events.Length == 0)
                {
                    serverlessLogger.LogWarning($"{patternLog} No event to process.");
                    return;
                }

                serverlessLogger.LogInformation($"{patternLog} Start processing...");

                var waitAndRetryAsync = GetWaitAndRetry(eventReliabilitySettings);

                var eventCount = events.Length;

                for (int eventIndex = 0; eventIndex < eventCount; eventIndex++)
                {
                    serverlessLogger.LogInformation($"{patternLog} Processing event index {eventIndex}...");

                    var eventData = events[eventIndex];
                    var fallbackAsync = GetFallback(serverlessLogger, patternLog, eventData, publishPoisonedEventAsync);

                    await Policy
                        .WrapAsync(fallbackAsync, waitAndRetryAsync)
                        .ExecuteAsync(() => processEventAsync(eventData))
                        .ConfigureAwait(false);

                    serverlessLogger.LogInformation($"{patternLog} Event index {eventIndex} processed!");
                }

                serverlessLogger.LogInformation($"{patternLog} Processing completed!");
            }
            catch (Exception ex)
            {
                serverlessLogger.LogError(ex, $"{patternLog} Processing failed!");
                throw;
            }
        }

        /// <summary>
        /// End event process logging notification messages.
        /// </summary>
        /// <typeparam name="TResult">Type of result data.</typeparam>
        /// <param name="result">Result data. If the object is null, throw exception and call the poisoned event function. Otherwise, the event was processed successfully.</param>
        /// <param name="correlationId">Correlation id.</param>
        /// <param name="notificationUseCase">Use case to extract notifications.</param>
        /// <param name="serverlessLogger">Logger to save log.</param>
        /// <param name="patternLog">Pattern log.</param>
        public static void EndEventProcess<TResult>(
            TResult result,
            string correlationId,
            DomainNotificationUseCase notificationUseCase,
            IServerlessLogger serverlessLogger,
            string patternLog)
            where TResult : IResult
        {
            if (result == null)
            {
                if (notificationUseCase.HasNotifications())
                {
                    var errors = GetAllMessageErrors(correlationId, notificationUseCase, patternLog);
                    throw new Exception(errors);
                }

                var message = $"{patternLog} Event item with {nameof(correlationId)}: {correlationId} processing failed.";
                throw new Exception(message);
            }

            serverlessLogger.LogInformation($"{patternLog} Event item with {nameof(correlationId)}: {correlationId} successfully processed!");
        }

        private static string GetAllMessageErrors(
            string correlationId,
            DomainNotificationUseCase notificationUseCase,
            string patternLog)
        {
            var errorsMessage = new StringBuilder();
            errorsMessage.Append($"{patternLog} Event item with {nameof(correlationId)}: {correlationId} processing failed.");

            if (notificationUseCase.HasNotifications())
            {
                notificationUseCase
                    .GetNotifications()?
                    .ForEach(n => n.Messages?.ToList()?.ForEach(m => errorsMessage.Append($"{n.MessageType}|{m.Key}: {m.Value};")));
            }

            return errorsMessage.ToString();
        }

        private static Polly.Retry.AsyncRetryPolicy GetWaitAndRetry(EventReliabilitySettings eventReliabilitySettings)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    (int)(eventReliabilitySettings?.RetryAttempts).GetValueOrDefault(),
                    retryAttempt => eventReliabilitySettings?.RetrySleepDuration ?? TimeSpan.FromSeconds(0));
        }

        private static Polly.Fallback.AsyncFallbackPolicy GetFallback(
            IServerlessLogger serverlessLogger,
            string patternLog,
            EventData eventData,
            Func<EventData, Exception, Task> publishPoisonedEventAsync)
        {
            return Policy
                .Handle<Exception>()
                .FallbackAsync(
                    (exception, context, cancellationToken) => publishPoisonedEventAsync(eventData, exception),
                    (exception, context) =>
                    {
                        serverlessLogger.LogError(exception, $"{patternLog} Processing failed!");
                        return Task.CompletedTask;
                    });
        }
    }
}