using System;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Middlewares
{
    /// <summary>
    /// Logger middleware to log function request and response using <see cref="IServerlessLogger"/>.
    /// </summary>
    public class LoggerMiddleware : HttpMiddleware
    {
        private readonly IServerlessLogger serverlessLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerMiddleware"/> class.
        /// </summary>
        /// <param name="serverlessLogger"></param>
        public LoggerMiddleware(IServerlessLogger serverlessLogger)
        {
            this.serverlessLogger = serverlessLogger;
        }

        /// <inheritdoc/>
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            var start = DateTime.UtcNow;

            try
            {
                await Next.InvokeAsync(context).ConfigureAwait(false);
            }
            finally
            {
                var end = DateTime.UtcNow;

                await serverlessLogger
                    .LogFunctionAsync(context.Request, context.Response, start, end)
                    .ConfigureAwait(false);
            }
        }
    }
}
