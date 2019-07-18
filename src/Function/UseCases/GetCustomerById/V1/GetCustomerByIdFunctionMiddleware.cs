using System;
using System.Threading.Tasks;
using AutoMapper;
using Clean.Microservice.Serverless.Core.Resources;
using Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Function.Middlewares.Helpers;
using Clean.Microservice.Serverless.SharedKernel.Function.UseCases.Presenters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Clean.Microservice.Serverless.Function.UseCases.GetCustomerById.V1
{
    public class GetCustomerByIdFunctionMiddleware : HttpMiddleware
    {
        private readonly HttpRequest req;
        private readonly string id;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly Presenter presenter;
        private readonly BadRequestPresenter badRequestPresenter;
        private readonly IServerlessLogger logger;

        public GetCustomerByIdFunctionMiddleware(
            HttpRequest req,
            string id,
            IMediator mediator,
            IMapper mapper,
            Presenter presenter,
            BadRequestPresenter badRequestPresenter,
            IServerlessLogger logger)
        {
            this.req = req;
            this.id = id;
            this.mediator = mediator;
            this.mapper = mapper;
            this.presenter = presenter;
            this.badRequestPresenter = badRequestPresenter;
            this.logger = logger;
        }

        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            logger.LogInformation($"[{nameof(GetCustomerByIdFunctionMiddleware)}] HTTP function processing a request.");

            if (!ValidateRequest(id))
            {
                context.Response = badRequestPresenter.ResponseModel;
                return;
            }

            var command = mapper.Map<GetCustomerByIdCommand>(Guid.Parse(id));

            var result = await mediator
                .Send(command)
                .ConfigureAwait(false);

            presenter.Populate(result);

            context.Response = presenter.ResponseModel;

            logger.LogInformation($"[{nameof(GetCustomerByIdFunctionMiddleware)}] HTTP function processed a request.");
        }

        private bool ValidateRequest(string id)
        {
            var isValid = true;

            if (!Guid.TryParse(id, out Guid guidId))
            {
                var fieldName = nameof(id);
                var message = string.Format(ValidationFields.FIELD_IS_REQUIRED, fieldName);

                badRequestPresenter.Populate(fieldName, message);
                return false;
            }

            return isValid;
        }
    }
}
