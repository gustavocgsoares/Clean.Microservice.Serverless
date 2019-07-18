using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Logging;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Results;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Helpers
{
    /// <summary>
    /// Helpers for cosmosdb trigger.
    /// </summary>
    public static class CosmosDbTriggerHelper
    {
        /// <summary>
        /// Create pattern log.
        /// </summary>
        /// <param name="className">Name of trigger class.</param>
        /// <param name="serverlessLogger">Logger to save log with pattern created and execution time.</param>
        /// <returns>Pattern log created.</returns>
        public static string CreatePatternLog(string className, IServerlessLogger serverlessLogger)
        {
            var pattern = $"[{Guid.NewGuid()} | {className}] ";

            serverlessLogger.LogInformation($"C# Cosmos trigger function executed at: {DateTime.UtcNow}");

            return pattern;
        }

        /// <summary>
        /// Extract correlationId value from <see cref="EventData"/> properties.
        /// </summary>
        /// <param name="serverlessLogger">Logger to save log with correlationId extracted.</param>
        /// <param name="patternLog">Pattern log.</param>
        /// <returns>CorrelationId value extracted.</returns>
        public static string GetCorrelationId(IServerlessLogger serverlessLogger, string patternLog)
        {
            var correlationId = Guid.NewGuid();

            serverlessLogger.LogInformation($"{patternLog} Processing document with {nameof(correlationId)}: {correlationId}!");

            return correlationId.ToString();
        }

        /// <summary>
        /// Start cosmosdb process.
        /// </summary>
        /// <param name="serverlessLogger">Logger to save log.</param>
        /// <param name="logger">Logger to be injected in <paramref name="serverlessLogger"/>.</param>
        /// <param name="patternLog">Pattern log.</param>
        /// <param name="documents">Documents to be processed.</param>
        /// <param name="func">Process with business rules to be processed.</param>
        /// <returns>Task executed.</returns>
        public static async Task StartDocumentProcessAsync(
            IServerlessLogger serverlessLogger,
            ILogger logger,
            string patternLog,
            IReadOnlyList<Document> documents,
            Func<Document, Task> func)
        {
            try
            {
                serverlessLogger.InjectLogger(logger);

                if (documents.Count == 0)
                {
                    serverlessLogger.LogWarning($"{patternLog} No document to process.");
                    return;
                }

                serverlessLogger.LogInformation($"{patternLog} Start processing...");

                var documentCount = documents.Count;

                for (int documentIndex = 0; documentIndex < documentCount; documentIndex++)
                {
                    var document = documents[documentIndex];
                    serverlessLogger.LogInformation($"{patternLog} Processing document index: {documentIndex}, id: {document.Id} and resourceId: {document.ResourceId}!");
                    await func(document).ConfigureAwait(false);
                    serverlessLogger.LogInformation($"{patternLog} Document index {documentIndex} processed!");
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
        /// End cosmosdb process logging notification messages.
        /// </summary>
        /// <typeparam name="TResult">Type of result data.</typeparam>
        /// <param name="result">Result data. If the object is null, throw exception. Otherwise, the trigger was processed successfully.</param>
        /// <param name="correlationId">Correlation id.</param>
        /// <param name="notificationUseCase">Use case to extract notifications.</param>
        /// <param name="serverlessLogger">Logger to save log.</param>
        /// <param name="patternLog">Pattern log.</param>
        public static void EndDocumentProcess<TResult>(
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

                throw new Exception($"{patternLog} Document item with {nameof(correlationId)}: {correlationId} processing failed.");
            }

            serverlessLogger.LogInformation($"{patternLog} Document item with {nameof(correlationId)}: {correlationId} successfully processed!");
        }

        private static string GetAllMessageErrors(
            string correlationId,
            DomainNotificationUseCase notificationUseCase,
            string patternLog)
        {
            var errorsMessage = new StringBuilder();
            errorsMessage.Append($"{patternLog} Document item with {nameof(correlationId)}: {correlationId} processing failed.");

            if (notificationUseCase.HasNotifications())
            {
                notificationUseCase
                    .GetNotifications()?
                    .ForEach(n => n.Messages?.ToList()?.ForEach(m => errorsMessage.Append($"{n.MessageType}|{m.Key}: {m.Value};")));
            }

            return errorsMessage.ToString();
        }
    }
}