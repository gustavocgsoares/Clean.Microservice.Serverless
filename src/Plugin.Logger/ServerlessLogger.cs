using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;
using Clean.Microservice.Serverless.Plugin.Logger.Entities;
using Clean.Microservice.Serverless.Plugin.Logger.Helpers;

namespace Clean.Microservice.Serverless.Plugin.Logger
{
    /// <summary>
    /// Serverless logger representation.
    /// </summary>
    [SuppressMessage("Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Not applicable")]
    public class ServerlessLogger : IServerlessLogger
    {
        private readonly ITransactionFlow transactionFlow;
        private readonly LogBlacklist logBlacklist;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerlessLogger"/> class.
        /// </summary>
        /// <param name="loggerFactory">See <see cref="ILoggerFactory"/>.</param>
        /// <param name="transactionFlow">See <see cref="ITransactionFlow"/>.</param>
        /// <param name="logBlacklist">Logger blacklist with sensitive fields to be masked.</param>
        public ServerlessLogger(
            ILoggerFactory loggerFactory,
            ITransactionFlow transactionFlow,
            LogBlacklist logBlacklist)
        {
            this.logBlacklist = logBlacklist;
            this.transactionFlow = transactionFlow;

            Logger = loggerFactory
                .CreateLogger<ServerlessLogger>();
        }

        private ILogger Logger { get; set; }

        /// <inheritdoc/>
        public async Task LogFunctionAsync(
            HttpRequest request,
            IActionResult response,
            DateTime start,
            DateTime end)
        {
            if (request != null)
            {
                var requestBody = FormatRequest(request).GetAwaiter().GetResult();
                var responseBody = ToJsonSerialize(response);

                SaveLogApi(requestBody, responseBody, start, end, request.HttpContext);
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task LogApiAsync(
            HttpContext context,
            Func<Task> func)
        {
            var start = DateTimeOffset.UtcNow;
            var originalBody = context?.Response?.Body;

            try
            {
                if (context != null && func != null)
                {
                    var requestBody = FormatRequest(context?.Request).GetAwaiter().GetResult();

                    using (var memStream = new MemoryStream())
                    {
                        context.Response.Body = memStream;

                        await func().ConfigureAwait(false);

                        var responseBody = FormatResponse(memStream, originalBody).GetAwaiter().GetResult();
                        var end = DateTimeOffset.UtcNow;
                        SaveLogApi(requestBody, responseBody, start, end, context);
                    }
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        /// <inheritdoc/>
        public void InjectLogger(ILogger logger)
        {
            if (logger != null)
            {
                Logger = logger;
            }
        }

        /// <inheritdoc/>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Logger.Log(logLevel, eventId, state, exception, formatter);
        }

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel)
        {
            return Logger.IsEnabled(logLevel);
        }

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state)
        {
            return Logger.BeginScope(state);
        }

        private string ToJsonSerialize(object data)
        {
            var result = string.Empty;

            try
            {
                result = data == null
                    ? string.Empty
                    : JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                this.LogError(ex, $"{nameof(ServerlessLogger)} -> Fail using {nameof(ToJsonSerialize)}: {ex.Message}");
            }

            return result?.MaskFields(logBlacklist);
        }

        private void SaveLogApi(
            string requestBody,
            string responseBody,
            DateTimeOffset start,
            DateTimeOffset end,
            HttpContext context = null)
        {
            try
            {
                var log = new LogApi(transactionFlow);
                log.CreateRequestData(context, requestBody, start, this);
                log.CreateResponseData(context, responseBody, end, this);

                this.LogInformation(log.Serialize());
            }
            catch (Exception ex)
            {
                this.LogError(ex, $"{nameof(ServerlessLogger)} -> Fail using {nameof(SaveLogApi)}: {ex.Message}");
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            string bodyAsText;

            // Allows using several time the stream in ASP.Net Core
            request.EnableRewind();

            // Arguments: Stream, Encoding, detect encoding, buffer size
            // AND, the most important: keep stream opened
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyAsText = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            // Rewind, so the core is not lost when it looks the body for the request
            request.Body.Position = 0;

            return bodyAsText?.MaskFields(logBlacklist);
        }

        private async Task<string> FormatResponse(MemoryStream stream, Stream originalBody)
        {
            string responseBody;

            stream.Position = 0;
            responseBody = await new StreamReader(stream).ReadToEndAsync().ConfigureAwait(false);
            stream.Position = 0;

            await stream.CopyToAsync(originalBody).ConfigureAwait(false);

            return responseBody?.MaskFields(logBlacklist);
        }

        private string GetExceptionSerialized<TException>(TException exception)
            where TException : Exception
        {
            string exceptionSerialized = null;

            try
            {
                var ex = Convert.ChangeType(exception, exception.GetType());
                exceptionSerialized = ToJsonSerialize(ex);
            }
            catch
            {
                try
                {
                    exceptionSerialized = ToJsonSerialize(exception);
                }
                catch (Exception ex)
                {
                    this.LogError(ex, $"{nameof(ServerlessLogger)} -> Fail using {nameof(GetExceptionSerialized)}: {ex.Message}");
                    var error = new { type = exception.GetType().FullName, message = exception.Message, stackTrace = exception.ToString() };
                    exceptionSerialized = ToJsonSerialize(error);
                }
            }

            return exceptionSerialized;
        }
    }
}