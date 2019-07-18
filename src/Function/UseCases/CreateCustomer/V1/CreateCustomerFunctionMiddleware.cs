using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers;
using Clean.Microservice.Serverless.SharedKernel.Function.Extensions;
using Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1.Models;
using Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models;
using Clean.Microservice.Serverless.SharedKernel.Function.UseCases.Presenters;
using System.Linq;

namespace Clean.Microservice.Serverless.Function.UseCases.CreateCustomer.V1
{
    public class CreateCustomerFunctionMiddleware : HttpMiddleware
    {
        private readonly HttpRequest req;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly Presenter presenter;
        private readonly BadRequestPresenter badRequestPresenter;
        private readonly IServerlessLogger logger;

        public CreateCustomerFunctionMiddleware(
            HttpRequest req,
            IMediator mediator,
            IMapper mapper,
            Presenter presenter,
            BadRequestPresenter badRequestPresenter,
            IServerlessLogger logger)
        {
            this.req = req;
            this.mediator = mediator;
            this.mapper = mapper;
            this.presenter = presenter;
            this.badRequestPresenter = badRequestPresenter;
            this.logger = logger;
        }

        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            logger.LogInformation($"[{nameof(CreateCustomerFunctionMiddleware)}] HTTP function processing a request.");

            var requestModel = await req.ExtractRequestBody<CreateCustomerRequestModel>()
                .ConfigureAwait(false);

            if (!ValidateRequest(requestModel))
            {
                context.Response = badRequestPresenter.ResponseModel;
                return;
            }

            var command = mapper.Map<CreateCustomerCommand>(requestModel);

            var result = await mediator.Send(command)
                .ConfigureAwait(false);

            presenter.Populate(result);

            context.Response = presenter.ResponseModel;

            logger.LogInformation($"[{nameof(CreateCustomerFunctionMiddleware)}] HTTP function processed a request.");
        }

        private bool ValidateRequest(CreateCustomerRequestModel requestModel)
        {
            var isValid = true;

            var validations = requestModel.GetValidations();

            if (validations.Any())
            {
                badRequestPresenter.Populate(validations);
                isValid = false;
            }

            return isValid;
        }
    }
}
