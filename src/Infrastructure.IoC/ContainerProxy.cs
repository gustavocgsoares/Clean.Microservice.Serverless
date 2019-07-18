using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.Core.Constants;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Presenters;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;
using Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC;
using Clean.Microservice.Serverless.Infrastructure.UseCases.GetCustomerById.V1;
using Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1;
using Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1.Models;

namespace Clean.Microservice.Serverless.Infrastructure.IoC
{
    public static class ContainerProxy
    {
        public static IServiceCollection AddApplicationServices<
            TSharedKernelPresenterAssembly,
            TPresenterAssembly>(
            this IServiceCollection services)
            where TSharedKernelPresenterAssembly : class, IPresenter
            where TPresenterAssembly : class, IPresenter
        {
            var loggerBlacklist = new List<string>();
            var instrumentationKey = Environment.GetEnvironmentVariable(EnvironmentConstants.AppInsightsInstrumentationKey);

            try
            {
                services.AddServices<
                    GetCustomerByIdUseCase,
                    GetCustomerByIdRepository,
                    TSharedKernelPresenterAssembly,
                    TPresenterAssembly,
                    GetCustomerByIdProfile,
                    FunctionTransactionFlow>(instrumentationKey, loggerBlacklist);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            services.AddDatabases();
            services.AddCache();
            services.AddVariables();

            return services;
        }
    }
}
