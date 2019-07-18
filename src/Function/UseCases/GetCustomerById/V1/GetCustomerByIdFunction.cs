using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Clean.Microservice.Serverless.Plugin.Logger;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.SharedKernel.Function.Extensions;
using Clean.Microservice.Serverless.SharedKernel.Function.UseCases.Presenters;

namespace Clean.Microservice.Serverless.Function.UseCases.GetCustomerById.V1
{
    public static class GetCustomerByIdFunction
    {
        [FunctionName(nameof(GetCustomerByIdAsync))]
        public static async Task<IActionResult> GetCustomerByIdAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/customers/{id}")] HttpRequest req,
            string id,
            [Inject] IMediator mediator,
            [Inject] IMapper mapper,
            [Inject] IServerlessLogger serverlessLogger,
            [Inject] IHttpContextAccessor acessor,
            [Inject] Presenter presenter,
            [Inject] BadRequestPresenter badRequestPresenter,
            ILogger log)
        {
            serverlessLogger.InjectLogger(log);

            var functionMiddleware = new GetCustomerByIdFunctionMiddleware(
                req,
                id,
                mediator,
                mapper,
                presenter,
                badRequestPresenter,
                serverlessLogger);

            return await functionMiddleware.ExecutePipeline(serverlessLogger, req, acessor, true)
                .ConfigureAwait(false);
        }
    }
}
