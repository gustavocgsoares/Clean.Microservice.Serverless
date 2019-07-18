using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Clean.Microservice.Serverless.Plugin.Logger;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers
{
    /// <summary>
    /// Class to middleware pipeline.
    /// </summary>
    public class MiddlewarePipeline : IMiddlewarePipeline
    {
        private readonly List<HttpMiddleware> pipeline = new List<HttpMiddleware>();

        private readonly IServerlessLogger serverlessLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MiddlewarePipeline"/> class.
        /// </summary>
        /// <param name="pipeline">List of <see cref="HttpMiddleware"/> to build a pipeline.</param>
        /// <param name="serverlessLogger">Logger to log error in case of fail.</param>
        public MiddlewarePipeline(List<HttpMiddleware> pipeline, IServerlessLogger serverlessLogger)
        {
            this.serverlessLogger = serverlessLogger ?? throw new ArgumentNullException(nameof(serverlessLogger));

            if (pipeline != null)
            {
                foreach (var httpMiddleware in pipeline)
                {
                    Register(httpMiddleware);
                }
            }
        }

        /// <inheritdoc/>
        public void Register(HttpMiddleware middleware)
        {
            if (pipeline.Any())
            {
                pipeline[pipeline.Count - 1].Next = middleware;
            }

            pipeline.Add(middleware);
        }

        /// <inheritdoc/>
        public List<HttpMiddleware> GetRegisteredPipeline()
        {
            return pipeline;
        }

        /// <inheritdoc/>
        public async Task<IActionResult> ExecuteAsync(IHttpFunctionContext context)
        {
            try
            {
                if (pipeline.Any())
                {
                    await pipeline[0].InvokeAsync(context).ConfigureAwait(false);

                    if (context.Response != null)
                    {
                        return context.Response;
                    }
                }

                throw new Exception();
            }
            catch (Exception e)
            {
                serverlessLogger.LogError(e, $"[{nameof(MiddlewarePipeline)}] {e.Message}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                throw;
            }
        }
    }
}
