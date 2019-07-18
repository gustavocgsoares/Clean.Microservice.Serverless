using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Presenters;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Repositories;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;
using Clean.Microservice.Serverless.SharedKernel.Core.Helpers;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC
{
    public static class ContainerProxy
    {
        public static IServiceCollection AddServices<
            TUseCaseAssembly,
            TRepositoryAssembly,
            TSharedKernelPresenterAssembly,
            TPresenterAssembly,
            TMapperProfileAssembly,
            TTransactionFlow>(
            this IServiceCollection services,
            string loggerAppInsightsInstrumentationKey,
            List<string> loggerBlacklist = null)
            where TUseCaseAssembly : UseCase
            where TRepositoryAssembly : class, IRepository
            where TSharedKernelPresenterAssembly : class, IPresenter
            where TPresenterAssembly : class, IPresenter
            where TMapperProfileAssembly : Profile
            where TTransactionFlow : class, ITransactionFlow
        {
            services.AddDomainServices();
            services.AddMediatr<TUseCaseAssembly>();
            services.AddRepositories<TRepositoryAssembly>();
            services.AddPresenters<TSharedKernelPresenterAssembly, TPresenterAssembly>();
            services.AddMappers<TMapperProfileAssembly>();
            services.AddLoggers(loggerAppInsightsInstrumentationKey, loggerBlacklist);
            services.AddTransactionFlow<TTransactionFlow>();
            ////services.AddDocumentations();

            return services;
        }

        public static IServiceCollection AddRedisCache(
            this IServiceCollection services,
            string connectionString,
            string instanceName)
        {
            services.AddScoped<ICacheHelper, CacheHelper>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = instanceName;
            });

            return services;
        }
    }
}
