using System.Threading.Tasks;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers
{
    /// <summary>
    /// Class to implement middleware pattern.
    /// </summary>
    public abstract class HttpMiddleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next <see cref="HttpMiddleware"/> to build a middleware pipeline.</param>
        protected HttpMiddleware(HttpMiddleware next)
        {
            Next = next;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMiddleware"/> class.
        /// </summary>
        protected HttpMiddleware()
        {
        }

        /// <summary>
        /// Gets or sets the next <see cref="HttpMiddleware"/> in pipeline execution.
        /// </summary>
        /// <value>
        /// Next <see cref="HttpMiddleware"/> in pipeline execution.
        /// </value>
        public HttpMiddleware Next { get; set; }

        /// <summary>
        /// Execute middleware.
        /// </summary>
        /// <param name="context">Function context.</param>
        /// <returns>Task result.</returns>
        public abstract Task InvokeAsync(IHttpFunctionContext context);
    }
}
