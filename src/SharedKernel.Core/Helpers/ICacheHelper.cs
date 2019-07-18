using System;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Helpers
{
    /// <summary>
    /// Cache helper interface.
    /// </summary>
    public interface ICacheHelper
    {
        /// <summary>
        /// Get data from cache or get data from main source and set cache.
        /// </summary>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <param name="originalSourceFuncAsync">Func to get data from original source.</param>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cacheExpirationTime">Cache expiration time</param>
        /// <returns>Return service response with result data or error.</returns>
        Task<ServiceResponse<TResult>> GetAsync<TResult>(
            Func<Task<ServiceResponse<TResult>>> originalSourceFuncAsync,
            string cacheKey,
            TimeSpan cacheExpirationTime);

        /// <summary>
        /// Get data from cache or get data from main source and set cache.
        /// </summary>
        /// <param name="originalSourceFuncAsync">Func to get data from original source.</param>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cacheExpirationTime">Cache expiration time</param>
        /// <returns>Return service response with result data or error.</returns>
        Task<ServiceResponse<byte[]>> GetByteAsync(
            Func<Task<ServiceResponse<byte[]>>> originalSourceFuncAsync,
            string cacheKey,
            TimeSpan cacheExpirationTime);

        /// <summary>
        /// Get data from cache.
        /// </summary>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <param name="cacheKey">Cache key.</param>
        /// <returns>Return service response with result data or error.</returns>
        Task<ServiceResponse<TResult>> GetAsync<TResult>(string cacheKey);

        /// <summary>
        /// Get data from cache.
        /// </summary>
        /// <param name="cacheKey">Cache key.</param>
        /// <returns>Return service response with result data or error.</returns>
        Task<ServiceResponse<byte[]>> GetByteAsync(string cacheKey);

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="data">Array data byte to be cached.</param>
        /// <param name="cacheExpirationTime">Cache expiration time.</param>
        void SetByteAsync(
            string cacheKey,
            byte[] data,
            TimeSpan cacheExpirationTime);

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <typeparam name="TData">Type of data.</typeparam>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="data">DAta to be cached.</param>
        /// <param name="cacheExpirationTime">Cache expiration time.</param>
        void SetStringAsync<TData>(
            string cacheKey,
            TData data,
            TimeSpan cacheExpirationTime);
    }
}
