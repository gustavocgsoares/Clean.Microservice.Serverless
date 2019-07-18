using System;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.Core.Domain.Entities;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Repositories;

namespace Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1
{
    public interface ICreateCustomerRepository : IRepository
    {
        Task<ServiceResponse<Guid>> CreateAsync(Customer customer);
    }
}
