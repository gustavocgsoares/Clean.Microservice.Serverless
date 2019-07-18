using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models;
using Newtonsoft.Json;
using Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers;
using Clean.Microservice.Serverless.SharedKernel.Function.Middlewares;

namespace Clean.Microservice.Serverless.SharedKernel.Function.Extensions
{
    /// <summary>
    /// Function extension methods.
    /// </summary>
    public static class FunctionExtensions
    {
        /// <summary>
        /// Extract request body converting to <typeparamref name="TRequestModel"/>.
        /// </summary>
        /// <typeparam name="TRequestModel">Type of request model with <see cref="IRequestModel"/> interface.</typeparam>
        /// <param name="request">Http request to extract body.</param>
        /// <param name="keepStreamOpen">True to leave the stream open after the <see cref="StreamReader"/> object is disposed; otherwise, false.</param>
        /// <returns>Converted <typeparamref name="TRequestModel"/> from request body.</returns>
        public static async Task<TRequestModel> ExtractRequestBody<TRequestModel>(this HttpRequest request, bool keepStreamOpen = true)
            where TRequestModel : IRequestModel
        {
            using (var stream = new StreamReader(request.Body, System.Text.Encoding.UTF8, true, Convert.ToInt32(request.ContentLength ?? 1024), keepStreamOpen))
            {
                var requestBody = await stream.ReadToEndAsync()
                    .ConfigureAwait(false);

                return JsonConvert.DeserializeObject<TRequestModel>(requestBody);
            }
        }

        /// <summary>
        /// Execute middleware pipeline.
        /// </summary>
        /// <param name="functionMiddleware">Function logic as middleware to be called inside pipeline.</param>
        /// <param name="serverlessLogger">Modal logger.</param>
        /// <param name="request">Function http request.</param>
        /// <param name="acessor">See <see cref="IHttpContextAccessor"/>.</param>
        /// <param name="allowAnonymous">If true, it allows access to the endpoint without authentication.(Default: false)</param>
        /// <returns>Response <see cref="IActionResult"/> after pipeline execution.</returns>
        public static async Task<IActionResult> ExecutePipeline(
            this HttpMiddleware functionMiddleware,
            IServerlessLogger serverlessLogger,
            HttpRequest request,
            IHttpContextAccessor acessor,
            bool allowAnonymous = false)
        {
            var middlewares = GetMiddlewares(functionMiddleware, serverlessLogger, acessor, allowAnonymous);

            return await new MiddlewarePipeline(middlewares, serverlessLogger)
                .ExecuteAsync(HttpFunctionContext.Build(request))
                .ConfigureAwait(false);
        }

        private static List<HttpMiddleware> GetMiddlewares(
            HttpMiddleware functionMiddleware,
            IServerlessLogger serverlessLogger,
            IHttpContextAccessor acessor,
            bool allowAnonymous)
        {
            var exceptionMiddleware = new ExceptionMiddleware(serverlessLogger);
            var loggerMiddleware = new LoggerMiddleware(serverlessLogger);
            var headerMiddleware = new HeaderMiddleware();

            List<HttpMiddleware> middlewares = null;

            if (allowAnonymous)
            {
                middlewares = MiddlewaresWithoutAuth(
                    functionMiddleware,
                    exceptionMiddleware,
                    loggerMiddleware,
                    headerMiddleware);
            }
            else
            {
                middlewares = MiddlewaresWithAuth(
                    functionMiddleware,
                    serverlessLogger,
                    acessor,
                    exceptionMiddleware,
                    loggerMiddleware,
                    headerMiddleware);
            }

            return middlewares;
        }

        private static List<HttpMiddleware> MiddlewaresWithAuth(HttpMiddleware functionMiddleware, IServerlessLogger serverlessLogger, IHttpContextAccessor acessor, ExceptionMiddleware exceptionMiddleware, LoggerMiddleware loggerMiddleware, HeaderMiddleware headerMiddleware)
        {
            var authenticationMiddleware = new AuthenticationMiddleware(
                                serverlessLogger,
                                Environment.GetEnvironmentVariable("AUTH.ISSUER"),
                                Environment.GetEnvironmentVariable("AUTH.AUDIENCE"),
                                Environment.GetEnvironmentVariable("AUTH.METADATAADDRESS"),
                                acessor);

            return new List<HttpMiddleware>()
            {
                loggerMiddleware,
                headerMiddleware,
                exceptionMiddleware,
                authenticationMiddleware,
                functionMiddleware
            };
        }

        private static List<HttpMiddleware> MiddlewaresWithoutAuth(HttpMiddleware functionMiddleware, ExceptionMiddleware exceptionMiddleware, LoggerMiddleware loggerMiddleware, HeaderMiddleware headerMiddleware)
        {
            return new List<HttpMiddleware>()
            {
                loggerMiddleware,
                headerMiddleware,
                exceptionMiddleware,
                functionMiddleware
            };
        }
    }
}