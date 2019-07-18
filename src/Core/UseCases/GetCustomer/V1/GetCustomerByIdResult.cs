using Clean.Microservice.Serverless.Core.Domain.Entities;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Results;

namespace Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1
{
    public class GetCustomerByIdResult : IResult
    {
        public GetCustomerByIdResult(Customer customer)
        {
            Customer = customer;
        }

        public Customer Customer { get; private set; }
    }
}
