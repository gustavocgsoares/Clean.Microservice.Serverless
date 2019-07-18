using System;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Domain
{
    /// <summary>
    /// Error data object.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets or sets error level.
        /// </summary>
        /// <value>
        /// Error level.
        /// </value>
        public ErrorLevel ErrorLevel { get; set; }

        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        /// <value>
        /// Error message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets error exception when exists.
        /// </summary>
        /// <value>
        /// Error exception when exists.
        /// </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets request data to simulate error case.
        /// </summary>
        /// <value>
        /// Request data to simulate error case.
        /// </value>
        public object Request { get; set; }

        /// <summary>
        /// Gets or sets response data.
        /// </summary>
        /// <value>
        /// Response data.
        /// </value>
        public object Response { get; set; }

        /// <summary>
        /// Gets or sets method name.
        /// </summary>
        /// <value>
        /// Method name.
        /// </value>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets error code.
        /// </summary>
        /// <value>
        /// Error code.
        /// </value>
        public string Code { get; set; }
    }
}
