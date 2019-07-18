using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers
{
    /// <summary>
    /// Interface to http function context.
    /// </summary>
    public interface IHttpFunctionContext
    {
        /// <summary>
        /// Gets http request with request values.
        /// </summary>
        /// <value>
        /// Http request with request values.
        /// </value>
        HttpRequest Request { get; }

        /// <summary>
        /// Gets or sets function response data.
        /// </summary>
        /// <value>
        /// Function response data.
        /// </value>
        IActionResult Response { get; set; }

        /// <summary>
        /// Gets logger interface.
        /// </summary>
        /// <value>
        /// Logger interface.
        /// </value>
        ILogger Logger { get; }
    }
}