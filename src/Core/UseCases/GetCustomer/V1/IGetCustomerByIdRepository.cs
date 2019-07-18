using System;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Repositories;

namespace Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1
{
    public interface IGetCustomerByIdRepository : IRepository
    {
        Task<ServiceResponse<Domain.Entities.Customer>> GetAsync(Guid id);
    }
}
