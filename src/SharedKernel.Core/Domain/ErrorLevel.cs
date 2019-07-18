namespace Clean.Microservice.Serverless.SharedKernel.Core.Domain
{
    /// <summary>
    /// Error levels.
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>
        /// Internal - When is an unpredictable error.
        /// </summary>
        Internal,

        /// <summary>
        /// Business - When is a predictable business error.
        /// </summary>
        Business
    }
}
