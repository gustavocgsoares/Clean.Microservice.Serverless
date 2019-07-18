using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers
{
    /// <summary>
    /// Interface to middleware pipeline.
    /// </summary>
    public interface IMiddlewarePipeline
    {
        /// <summary>
        /// Register a pipeline.
        /// </summary>
        /// <param name="middleware">Next middleware in pipeline.</param>
        void Register(HttpMiddleware middleware);

        /// <summary>
        /// Get registered pipeline.
        /// </summary>
        /// <returns>Registered pipeline.</returns>
        List<HttpMiddleware> GetRegisteredPipeline();

        /// <summary>
        /// Execute pipeline.
        /// </summary>
        /// <param name="context">Http function context.</param>
        /// <returns>Function response data.</returns>
        Task<IActionResult> ExecuteAsync(IHttpFunctionContext context);
    }
}
