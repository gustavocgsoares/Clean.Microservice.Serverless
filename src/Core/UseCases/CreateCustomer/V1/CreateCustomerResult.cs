using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Results;
using System;

namespace Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1
{
    public class CreateCustomerResult : IResult
    {
        public CreateCustomerResult(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
