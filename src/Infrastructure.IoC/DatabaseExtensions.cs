using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.Plugin.Database;
using Clean.Microservice.Serverless.Core.Constants;
using Clean.Microservice.Serverless.Plugin.Database.SqlServer;

namespace Clean.Microservice.Serverless.Infrastructure.IoC
{
    internal static class DatabaseExtensions
    {
        public static void AddDatabases(this IServiceCollection services)
        {
            services.AddSqlServer();
        }

        private static void AddSqlServer(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable(EnvironmentConstants.ConnectionStringSqlCustomer);

            var databaseSettings = new List<DatabaseSettings>
            {
                DatabaseSettings.Builder(EnvironmentConstants.ConnectionStringSqlCustomer, connectionString)
            };

            services.AddTransient<IDatabaseFactory<SqlServerDatabase>>(s =>
                new DatabaseFactory<SqlServerDatabase>(databaseSettings, EnvironmentConstants.ConnectionStringSqlCustomer));
        }
    }
}
