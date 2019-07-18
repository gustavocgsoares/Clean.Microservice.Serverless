using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Clean.Microservice.Serverless.Plugin.Logger
{
    /// <summary>
    /// Interface to logger.
    /// </summary>
    public interface IServerlessLogger : ILogger
    {
        /// <summary>
        /// Inject <see cref="ILogger"/> sent by function method to link request logs.
        /// </summary>
        /// <remarks>This method is only required for azure function APIs.</remarks>
        /// <param name="logger"><see cref="ILogger"/> sent by function method.</param>
        void InjectLogger(ILogger logger);

        /// <summary>
        /// Log request and response for Web Api.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/> request context.</param>
        /// <param name="func">Injected function with request core method.</param>
        /// <returns>Task result.</returns>
        Task LogApiAsync(HttpContext context, Func<Task> func);

        /// <summary>
        /// Log request and response for Azure Function Api.
        /// </summary>
        /// <param name="request"><see cref="HttpRequest"/> with request data.</param>
        /// <param name="response"><see cref="IActionResult"/> response data.</param>
        /// <param name="start">Date and time when request started.</param>
        /// <param name="end">Date and time when request ended.</param>
        /// <returns>Task result.</returns>
        Task LogFunctionAsync(HttpRequest request, IActionResult response, DateTime start, DateTime end);
    }
}
