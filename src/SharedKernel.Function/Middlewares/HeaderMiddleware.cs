using System;
using System.Linq;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Middlewares
{
    /// <summary>
    /// Header middleware to manager correlation id and request id.
    /// </summary>
    public class HeaderMiddleware : HttpMiddleware
    {
        /// <inheritdoc/>
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            SetHeaderWithCorrelationId(context);
            SetHeaderWithRequestId(context);

            await Next.InvokeAsync(context).ConfigureAwait(false);
        }

        private static void SetHeaderWithCorrelationId(IHttpFunctionContext context)
        {
            var request = context.Request;
            var response = request.HttpContext.Response;
            var correlationId = Guid.NewGuid().ToString();
            var containsKey = request.Headers.ContainsKey(Constants.HeaderNameCorrelationId);

            if (!containsKey)
            {
                response.Headers.Add(Constants.HeaderNameCorrelationId, correlationId);
                return;
            }

            request.Headers.TryGetValue(Constants.HeaderNameCorrelationId, out var correlationHeader);

            if (string.IsNullOrWhiteSpace(correlationHeader.FirstOrDefault()))
            {
                response.Headers.Remove(Constants.HeaderNameCorrelationId);
                response.Headers.Add(Constants.HeaderNameCorrelationId, correlationId);
                return;
            }

            response.Headers.Add(Constants.HeaderNameCorrelationId, request.Headers[Constants.HeaderNameCorrelationId].ToString());
        }

        private static void SetHeaderWithRequestId(IHttpFunctionContext context)
        {
            var request = context.Request;
            var requestId = Guid.NewGuid().ToString();
            var containsKey = request.Headers.ContainsKey(Constants.HeaderNameRequestId);

            if (!containsKey)
            {
                request.Headers.Add(Constants.HeaderNameRequestId, requestId);
                return;
            }

            request.Headers.TryGetValue(Constants.HeaderNameRequestId, out var requestIdHeader);

            if (string.IsNullOrWhiteSpace(requestIdHeader.FirstOrDefault()))
            {
                request.Headers.Remove(Constants.HeaderNameRequestId);
                request.Headers.Add(Constants.HeaderNameRequestId, requestId);
            }
        }
    }
}
