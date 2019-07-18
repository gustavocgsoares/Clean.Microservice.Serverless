using System;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.Core.Domain.Entities;
using Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1;
using Clean.Microservice.Serverless.Plugin.Database;
using Clean.Microservice.Serverless.Plugin.Database.SqlServer;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;
using Dapper;

namespace Clean.Microservice.Serverless.Infrastructure.UseCases.CreateCustomer.V1
{
    public class CreateCustomerRepository : ICreateCustomerRepository
    {
        private readonly SqlServerDatabase sqlDatabase;
        private readonly IServerlessLogger logger;

        public CreateCustomerRepository(
            IDatabaseFactory<SqlServerDatabase> sqlDatabase,
            IServerlessLogger logger)
        {
            this.sqlDatabase = sqlDatabase.Create();
            this.logger = logger;
        }

        public async Task<ServiceResponse<Guid>> CreateAsync(Customer customer)
        {
            var parameters = new
            {
                name = customer.Name,
                birthDate = customer.BirthDate,
                email = customer.Contacts.Email,
                phone = customer.Contacts.Phone,
                createdAt = customer.CreatedAt
            };

            Guid result;

            using (var con = sqlDatabase.Connection)
            {
                result = await Task.FromResult(con.ExecuteScalar<Guid>(CommandQuery.CreateCustomer, parameters))
                    .ConfigureAwait(false);
            }

            return ServiceResponseHelper.WithResult(result);
        }
    }
}
