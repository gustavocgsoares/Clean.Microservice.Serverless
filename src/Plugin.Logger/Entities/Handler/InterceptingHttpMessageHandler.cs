using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.Plugin.Logger.Entities.Behaviors;

namespace Clean.Microservice.Serverless.Plugin.Logger.Entities.Handler
{
    /// <summary>
    /// Intercept delegate handler.
    /// </summary>
    public class InterceptingHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpMessageHandlerBehavior parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptingHttpMessageHandler"/> class.
        /// </summary>
        /// <param name="innerHandler"></param>
        /// <param name="parent"></param>
        public InterceptingHttpMessageHandler(HttpMessageHandler innerHandler, HttpMessageHandlerBehavior parent)
        {
            InnerHandler = innerHandler;
            this.parent = parent;
        }

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            if (parent.OnSending != null)
            {
                response = parent.OnSending(request, cancellationToken);
                if (response != null)
                {
                    return response;
                }
            }

            response = await base.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (parent.OnSent != null)
            {
                return parent.OnSent(response, cancellationToken);
            }

            return response;
        }
    }
}
