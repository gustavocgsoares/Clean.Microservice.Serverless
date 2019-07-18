using AutoMapper;
using MediatR;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases;
using System.Threading;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;
using Clean.Microservice.Serverless.Core.Domain.Entities;

namespace Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1
{
    public sealed class CreateCustomerUseCase : UseCase,
       IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
    {
        private readonly IMapper mapper;
        private readonly IServerlessLogger logger;
        private readonly ITransactionFlow transactionFlow;
        private readonly ICreateCustomerRepository createCustomerRepository;

        public CreateCustomerUseCase(
            IMediator mediator,
            IMapper mapper,
            IServerlessLogger logger,
            ITransactionFlow transactionFlow,
            ICreateCustomerRepository createCustomerRepository)
            : base(mediator, logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.transactionFlow = transactionFlow;
            this.createCustomerRepository = createCustomerRepository;
        }

        private CreateCustomerResult ErrorResult { get; } = default(CreateCustomerResult);

        public async Task<CreateCustomerResult> Handle(CreateCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!(message?.IsValid()).GetValueOrDefault())
            {
                NotifyValidationErrors(message);
                return ErrorResult;
            }

            var entity = mapper.Map<Customer>(message);

            var response = await createCustomerRepository
                .CreateAsync(entity)
                .ConfigureAwait(false);

            if (response.HasError)
            {
                NotifyError(response.Error);
                return ErrorResult;
            }

            return mapper.Map<CreateCustomerResult>(response.Result);
        }
    }
}
