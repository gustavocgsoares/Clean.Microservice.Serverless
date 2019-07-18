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

namespace Clean.Microservice.Serverless.Function.UseCases.CreateCustomer.V1
{
    public static class CreateCustomerFunction
    {
        [FunctionName(nameof(CreateCustomerAsync))]
        public static async Task<IActionResult> CreateCustomerAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/customers")] HttpRequest req,
            [Inject] IMediator mediator,
            [Inject] IMapper mapper,
            [Inject] IServerlessLogger serverlessLogger,
            [Inject] IHttpContextAccessor acessor,
            [Inject] Presenter presenter,
            [Inject] BadRequestPresenter badRequestPresenter,
            ILogger log)
        {
            serverlessLogger.InjectLogger(log);

            var functionMiddleware = new CreateCustomerFunctionMiddleware(
                req,
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
