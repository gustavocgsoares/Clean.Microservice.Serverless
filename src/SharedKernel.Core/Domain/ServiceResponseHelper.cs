using System;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Domain
{
    /// <summary>
    /// Service response helper.
    /// </summary>
    public static class ServiceResponseHelper
    {
        /// <summary>
        /// Generate service response with result.
        /// </summary>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <param name="result">Result for successful request.</param>
        /// <returns>Service response generated.</returns>
        public static ServiceResponse<TResult> WithResult<TResult>(TResult result)
        {
            return new ServiceResponse<TResult>
            {
                Result = result
            };
        }

        /// <summary>
        /// Generate service response with error result.
        /// </summary>
        /// <remarks>If error level not be setted the default value will be <see cref="ErrorLevel.Internal"/>.</remarks>
        /// <exception cref="ArgumentNullException">When error is null or error message is null or empty.</exception>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <param name="error">Error data.</param>
        /// <returns>Service response with error result.</returns>
        public static ServiceResponse<TResult> WithError<TResult>(Error error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            if (string.IsNullOrWhiteSpace(error.Message))
            {
                throw new ArgumentNullException($"{nameof(error.Message)} is required.");
            }

            var response = new ServiceResponse<TResult>
            {
                Error = error
            };

            return response;
        }

        /// <summary>
        /// Generate service response with error result.
        /// </summary>
        /// <exception cref="ArgumentNullException">When message is null or empty.</exception>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <param name="message">Business error message.</param>
        /// <returns>Service response with error result.</returns>
        public static ServiceResponse<TResult> WithBusinessErrorMessage<TResult>(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            var response = new ServiceResponse<TResult>
            {
                Error = new Error
                {
                    ErrorLevel = ErrorLevel.Business,
                    Message = message
                }
            };

            return response;
        }
    }
}
