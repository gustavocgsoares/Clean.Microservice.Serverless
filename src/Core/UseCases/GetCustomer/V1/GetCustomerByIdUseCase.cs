using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases;
using MediatR;

namespace Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1
{
    public sealed class GetCustomerByIdUseCase : UseCase,
       IRequestHandler<GetCustomerByIdCommand, GetCustomerByIdResult>
    {
        private readonly IMapper mapper;
        private readonly IServerlessLogger logger;
        private readonly ITransactionFlow transactionFlow;
        private readonly IGetCustomerByIdRepository getCustomerByIdRepository;

        public GetCustomerByIdUseCase(
            IMediator mediator,
            IMapper mapper,
            IServerlessLogger logger,
            ITransactionFlow transactionFlow,
            IGetCustomerByIdRepository getCustomerByIdRepository)
            : base(mediator, logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.transactionFlow = transactionFlow;
            this.getCustomerByIdRepository = getCustomerByIdRepository;
        }

        private GetCustomerByIdResult ErrorResult { get; } = default(GetCustomerByIdResult);

        public async Task<GetCustomerByIdResult> Handle(GetCustomerByIdCommand message, CancellationToken cancellationToken)
        {
            if (!(message?.IsValid()).GetValueOrDefault())
            {
                NotifyValidationErrors(message);
                return ErrorResult;
            }

            var response = await getCustomerByIdRepository
                .GetAsync(message.Id)
                .ConfigureAwait(false);

            if (response.HasError)
            {
                NotifyError(response.Error);
                return ErrorResult;
            }

            return mapper.Map<GetCustomerByIdResult>(response.Result);
        }
    }
}
