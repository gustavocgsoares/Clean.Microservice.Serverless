using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers
{
    /// <summary>
    /// Class with http function context.
    /// </summary>
    public class HttpFunctionContext : IHttpFunctionContext
    {
        /// <inheritdoc/>
        public HttpRequest Request { get; private set; }

        /// <inheritdoc/>
        public IActionResult Response { get; set; }

        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <summary>
        /// Build a <see cref="HttpFunctionContext"/> from <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">See <see cref="HttpRequest"/> data.</param>
        /// <returns><see cref="HttpFunctionContext"/> built.</returns>
        public static HttpFunctionContext Build(HttpRequest request)
        {
            return new HttpFunctionContext
            {
                Request = request
            };
        }
    }
}
