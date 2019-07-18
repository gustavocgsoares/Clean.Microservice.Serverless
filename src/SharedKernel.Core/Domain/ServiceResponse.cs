namespace Clean.Microservice.Serverless.SharedKernel.Core.Domain
{
    /// <summary>
    /// Service response used to avoid using exceptions.
    /// </summary>
    /// <typeparam name="T">Type of result.</typeparam>
    public class ServiceResponse<T>
    {
        /// <summary>
        /// Gets or sets error data when exists.
        /// </summary>
        /// <value>
        /// Error data when exists.
        /// </value>
        public Error Error { get; set; }

        /// <summary>
        /// Gets or sets data for successful requests.
        /// </summary>
        /// <value>
        /// result data.
        /// </value>
        public T Result { get; set; }

        /// <summary>
        /// Gets a value indicating whether has error data.
        /// </summary>
        /// <value>
        /// A value indicating whether has error data.
        /// </value>
        public bool HasError
        {
            get
            {
                return Error != null;
            }
        }
    }
}
