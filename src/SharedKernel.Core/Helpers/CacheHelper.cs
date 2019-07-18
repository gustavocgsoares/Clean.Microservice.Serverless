using System;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;
using Clean.Microservice.Serverless.SharedKernel.Core.Resources;
using Clean.Microservice.Serverless.SharedKernel.Core.Serializers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Clean.Microservice.Serverless.SharedKernel.Core.Helpers
{
    /// <summary>
    /// Cache helper using IDistributedCache.
    /// </summary>
    public class CacheHelper : ICacheHelper
    {
        private readonly ILogger logger;
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheHelper"/> class.
        /// </summary>
        /// <param name="logger">See <see cref="ILogger"/>.</param>
        /// <param name="distributedCache">See <see cref="IDistributedCache"/>.</param>
        public CacheHelper(ILogger logger, IDistributedCache distributedCache)
        {
            this.logger = logger;
            this.distributedCache = distributedCache;
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<TResult>> GetAsync<TResult>(
            Func<Task<ServiceResponse<TResult>>> originalSourceFuncAsync,
            string cacheKey,
            TimeSpan cacheExpirationTime)
        {
            var serviceResponse = await GetAsync<TResult>(cacheKey)
                .ConfigureAwait(false);

            if (serviceResponse.HasError || serviceResponse.Result == null)
            {
                serviceResponse = await originalSourceFuncAsync()
                    .ConfigureAwait(false);

                if (serviceResponse.HasError)
                {
                    return serviceResponse;
                }

                SetStringAsync(cacheKey, serviceResponse.Result, cacheExpirationTime);
            }

            return serviceResponse;
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<TResult>> GetAsync<TResult>(string cacheKey)
        {
            string cacheResult = null;

            try
            {
                cacheResult = await distributedCache
                    .GetStringAsync(cacheKey)
                    .ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(cacheResult))
                {
                    return ServiceResponseHelper.WithResult(default(TResult));
                }

                var settings = new JsonSerializerSettings().GetJsonSerializerSettingsWithPrivateCamelCaseSerializer();
                var deserializedObject = JsonConvert.DeserializeObject<TResult>(cacheResult, settings);
                return ServiceResponseHelper.WithResult(deserializedObject);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[{nameof(CacheHelper)}] Fail using {nameof(GetAsync)} for result {typeof(TResult).FullName}: {ex.Message}");
                return ServiceResponseHelper.WithError<TResult>(new Error { Message = HttpMessage.CACHE_MESSAGE_FAIL, Exception = ex });
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<byte[]>> GetByteAsync(
            Func<Task<ServiceResponse<byte[]>>> originalSourceFuncAsync,
            string cacheKey,
            TimeSpan cacheExpirationTime)
        {
            var serviceResponse = await GetByteAsync(cacheKey)
                .ConfigureAwait(false);

            if (serviceResponse.HasError || serviceResponse.Result == null)
            {
                serviceResponse = await originalSourceFuncAsync()
                    .ConfigureAwait(false);

                if (serviceResponse.HasError)
                {
                    return serviceResponse;
                }

                SetByteAsync(cacheKey, serviceResponse.Result, cacheExpirationTime);
            }

            return serviceResponse;
        }

        /// <inheritdoc/>
        public async Task<ServiceResponse<byte[]>> GetByteAsync(string cacheKey)
        {
            byte[] cacheResult = null;

            try
            {
                cacheResult = await distributedCache
                    .GetAsync(cacheKey)
                    .ConfigureAwait(false);

                if (cacheResult == null)
                {
                    return ServiceResponseHelper.WithResult(default(byte[]));
                }

                return ServiceResponseHelper.WithResult(cacheResult);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[{nameof(CacheHelper)}] Fail using {nameof(GetByteAsync)} for result {typeof(byte[]).FullName}: {ex.Message}");
                return ServiceResponseHelper.WithError<byte[]>(new Error { Message = HttpMessage.CACHE_MESSAGE_FAIL, Exception = ex });
            }
        }

        /// <inheritdoc/>
        public void SetByteAsync(string cacheKey, byte[] data, TimeSpan cacheExpirationTime)
        {
            try
            {
                if (data == null)
                {
                    return;
                }

                DistributedCacheEntryOptions options = GetCacheOptions(cacheExpirationTime);

                distributedCache
                    .SetAsync(cacheKey, data, options)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[{nameof(CacheHelper)}] Fail using {nameof(SetByteAsync)}: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public void SetStringAsync<TData>(string cacheKey, TData data, TimeSpan cacheExpirationTime)
        {
            try
            {
                if (data == null)
                {
                    return;
                }

                string serializedData = GetSerializedData(data);
                DistributedCacheEntryOptions options = GetCacheOptions(cacheExpirationTime);

                distributedCache
                    .SetStringAsync(cacheKey, serializedData, options)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[{nameof(CacheHelper)}] Fail using {nameof(SetStringAsync)}: {ex.Message}");
            }
        }

        private static DistributedCacheEntryOptions GetCacheOptions(TimeSpan cacheExpirationTime)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheExpirationTime
            };
        }

        private static string GetSerializedData(object data)
        {
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(data, Formatting.None, jsonSerializeSettings);
        }
    }
}
