using System;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Commands;

namespace Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1
{
    public class GetCustomerByIdCommand : Command<GetCustomerByIdResult>
    {
        public GetCustomerByIdCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public override bool IsValid()
        {
            ValidationResult = new GetCustomerByIdCommandValidator()
                .Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
