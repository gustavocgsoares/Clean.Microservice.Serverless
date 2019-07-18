namespace Clean.Microservice.Serverless.SharedKernel.Function
{
    /// <summary>
    /// Function base constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Header name of X-Correlation-Id
        /// </summary>
        public const string HeaderNameCorrelationId = "X-Correlation-Id";

        /// <summary>
        /// Header name of X-Request-Id
        /// </summary>
        public const string HeaderNameRequestId = "X-Request-Id";

        /// <summary>
        /// Header name of X-Detailed-Error
        /// </summary>
        public const string HeaderNameDetailedError = "X-Detailed-Error";

        /// <summary>
        /// Client Ip Address
        /// </summary>
        public const string ServerVariablesFowardedFor = "HTTP_X_FORWARDED_FOR";

        /// <summary>
        /// Ip Address
        /// </summary>
        public const string ServerVariablesRemoteAddr = "REMOTE_ADDR";
    }
}
