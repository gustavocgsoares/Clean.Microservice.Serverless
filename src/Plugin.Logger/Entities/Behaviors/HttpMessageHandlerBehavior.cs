using System;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading;
using Clean.Microservice.Serverless.Plugin.Logger.Entities.Handler;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;

namespace Clean.Microservice.Serverless.Plugin.Logger.Entities.Behaviors
{
    /// <summary>
    /// Log Behavior.
    /// </summary>
    public class HttpMessageHandlerBehavior : IEndpointBehavior
    {
        private readonly ITransactionFlow transactionFlow;
        private readonly IServerlessLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageHandlerBehavior"/> class.
        /// </summary>
        /// <param name="transactionFlow"></param>
        /// <param name="logger"></param>
        public HttpMessageHandlerBehavior(ITransactionFlow transactionFlow, IServerlessLogger logger)
        {
            this.transactionFlow = transactionFlow;
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets OnSending method.
        /// </summary>
        /// <value>
        /// OnSending method.
        /// </value>
        public Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> OnSending { get; set; }

        /// <summary>
        /// Gets or sets OnSent method.
        /// </summary>
        /// <value>
        /// OnSent method.
        /// </value>
        public Func<HttpResponseMessage, CancellationToken, HttpResponseMessage> OnSent { get; set; }

        /// <inheritdoc/>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            if (bindingParameters != null)
            {
                bindingParameters.Add(new Func<HttpClientHandler, HttpMessageHandler>(GetHttpMessageHandler));
            }
        }

        /// <inheritdoc/>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (clientRuntime != null)
            {
                clientRuntime.ClientMessageInspectors.Add(new LogWcf(logger, transactionFlow));
            }
        }

        /// <inheritdoc/>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <inheritdoc/>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// Call intercept handler.
        /// </summary>
        /// <param name="httpClientHandler"></param>
        /// <returns>Intercept http message handler.</returns>
        public HttpMessageHandler GetHttpMessageHandler(HttpClientHandler httpClientHandler)
        {
            return new InterceptingHttpMessageHandler(httpClientHandler, this);
        }
    }
}
