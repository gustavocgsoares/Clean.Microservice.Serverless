using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Clean.Microservice.Serverless.Plugin.Logger.Helpers;
using System.Diagnostics.CodeAnalysis;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;

namespace Clean.Microservice.Serverless.Plugin.Logger
{
    /// <summary>
    /// Logging handler to log third party requests.
    /// </summary>
    [SuppressMessage("Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Not applicable")]
    public class LoggingHandler : DelegatingHandler
    {
        private readonly IServerlessLogger serverlessLogger;
        private readonly ITransactionFlow transactionFlow;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingHandler"/> class.
        /// </summary>
        /// <param name="serverlessLogger">See <see cref="IServerlessLogger"/>.</param>
        /// <param name="transactionFlow">See <see cref="ITransactionFlow"/>.</param>
        public LoggingHandler(
            IServerlessLogger serverlessLogger,
            ITransactionFlow transactionFlow)
        {
            this.serverlessLogger = serverlessLogger;
            this.transactionFlow = transactionFlow;
        }

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            var requestText = string.Empty;
            var responseText = string.Empty;

            if (request != null && request.Content != null)
            {
                requestText = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            try
            {
                var startProcess = DateTimeOffset.UtcNow;
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                var endProcess = DateTimeOffset.UtcNow;

                if (response.Content != null)
                {
                    responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }

                var log = CreateLog(
                    startProcess,
                    endProcess,
                    request,
                    response,
                    requestText,
                    responseText);

                await Task.Run(() =>
                {
                    serverlessLogger.LogInformation(Serialize(log));
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                serverlessLogger.LogError(ex, $"{nameof(ServerlessLogger)}.{nameof(LoggingHandler)} -> Fail to log Third Party: {ex.Message}");
            }

            return response;
        }

        private object CreateLog(
            DateTimeOffset startProcess,
            DateTimeOffset endProcess,
            HttpRequestMessage request,
            HttpResponseMessage response,
            string requestText,
            string responseText)
        {
            return new
            {
                type = "Third Party",
                date = DateTimeOffset.UtcNow,
                authUserId = transactionFlow.Id,
                clientId = transactionFlow.ClientId,
                customerId = transactionFlow.CustomerId,
                customerDocument = transactionFlow.CustomerDocument,
                startTimestamp = startProcess,
                endTimestamp = endProcess,
                duration = Convert.ToDecimal(Math.Round((endProcess - startProcess).TotalMilliseconds, 2, MidpointRounding.AwayFromZero)),
                culture = transactionFlow.Culture,
                url = request.RequestUri.AbsoluteUri,
                machine = transactionFlow.Machine,
                hostIp = transactionFlow.HostIp,
                localIp = transactionFlow.LocalIp,
                serviceType = "rest",
                request = new
                {
                    body = LoggerHelper.ConvertToJson(requestText, serverlessLogger),
                    method = request.Method.Method,
                    headers = request.Headers.ToDictionary(h => h.Key, h => h.Value),
                },
                response = new
                {
                    body = LoggerHelper.ConvertToJson(responseText, serverlessLogger),
                    headers = response.Headers.ToDictionary(h => h.Key, h => h.Value),
                    isSuccessStatusCode = response.IsSuccessStatusCode,
                    statusCode = response.StatusCode.GetHashCode(),
                },
                method = request.Method.Method,
                hasError = !response.IsSuccessStatusCode,
            };
        }

        private string Serialize(object log)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter(),
                },
            };

            return JsonConvert.SerializeObject(log, settings);
        }
    }
}
