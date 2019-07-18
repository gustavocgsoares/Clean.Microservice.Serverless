using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers;
using Clean.Microservice.Serverless.SharedKernel.Function.Resources;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Middlewares
{
    /// <summary>
    /// Exception middleware.
    /// </summary>
    [SuppressMessage("Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Not necessary")]
    public class ExceptionMiddleware : HttpMiddleware
    {
        private readonly IServerlessLogger serverlessLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="serverlessLogger">Logger to log error.</param>
        public ExceptionMiddleware(IServerlessLogger serverlessLogger)
        {
            this.serverlessLogger = serverlessLogger ?? throw new ArgumentNullException(nameof(serverlessLogger));
        }

        /// <inheritdoc/>
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            var hasError = true;
            Exception exception = null;

            try
            {
                await Next.InvokeAsync(context).ConfigureAwait(false);
                hasError = false;
            }
            catch (UnauthorizedAccessException ex)
            {
                exception = ex;
                context.Response = new UnauthorizedResult();
            }
            catch (Exception ex)
            {
                exception = ex;
                context.Response = GetUnpredictableResult(context.Request, ex);
            }
            finally
            {
                if (hasError)
                {
                    serverlessLogger.LogError(exception, $"[{nameof(ExceptionMiddleware)}]");
                }
            }
        }

        private ObjectResult GetUnpredictableResult(HttpRequest request, Exception exception)
        {
            request.Headers.TryGetValue(Constants.HeaderNameDetailedError, out var detailedError);
            request.HttpContext.Response.Headers.TryGetValue(Constants.HeaderNameCorrelationId, out var correlationId);

            var valueToCheck = DateTimeOffset.UtcNow.ToString("yyyyMM");
            var showStack = detailedError.FirstOrDefault()?.Equals(valueToCheck, StringComparison.CurrentCultureIgnoreCase);

            return showStack.GetValueOrDefault()
                 ? new ObjectResult(new { code = Messages.UNPREDICTABLE_CODE, correlationId = correlationId.FirstOrDefault(), message = Messages.UNPREDICTABLE_MESSAGE, stack = exception }) { StatusCode = 500 }
                 : new ObjectResult(new { code = Messages.UNPREDICTABLE_CODE, correlationId = correlationId.FirstOrDefault(), message = Messages.UNPREDICTABLE_MESSAGE }) { StatusCode = 500 };
        }
    }
}
