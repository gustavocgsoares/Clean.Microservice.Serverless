using System;
using System.Linq;
using System.Threading.Tasks;
using Clean.Microservice.Serverless.Core.Domain.Entities;
using Clean.Microservice.Serverless.Core.Domain.ValueObjects;
using Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1;
using Clean.Microservice.Serverless.Plugin.Database;
using Clean.Microservice.Serverless.Plugin.Database.SqlServer;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;
using Dapper;

namespace Clean.Microservice.Serverless.Infrastructure.UseCases.GetCustomerById.V1
{
    public class GetCustomerByIdRepository : IGetCustomerByIdRepository
    {
        private readonly SqlServerDatabase sqlDatabase;
        private readonly IServerlessLogger logger;

        public GetCustomerByIdRepository(
            IDatabaseFactory<SqlServerDatabase> sqlDatabase,
            IServerlessLogger logger)
        {
            this.sqlDatabase = sqlDatabase.Create();
            this.logger = logger;
        }

        public async Task<ServiceResponse<Customer>> GetAsync(Guid id)
        {
            Customer result = null;

            using (var con = sqlDatabase.Connection)
            {
                var customers = await con.QueryAsync<Customer, CustomerContactVO, Customer>(
                        CommandQuery.GetCustomerById,
                        (customer, contact) =>
                        {
                            return Customer.Builder(customer, contact);
                        },
                        new { id },
                        splitOn: "Email")
                    .ConfigureAwait(false);

                if (customers.Any())
                {
                    result = customers?.FirstOrDefault();
                }
            }

            return new ServiceResponse<Customer> { Result = result };
        }
    }
}