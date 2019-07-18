using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Repositories;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC
{
    internal static class RepositoryExtensions
    {
        public static void AddRepositories<TRepositoryAssembly>(this IServiceCollection services)
            where TRepositoryAssembly : class, IRepository
        {
            services.Scan(config =>
            {
                config.FromAssemblyOf<TRepositoryAssembly>()
                    .AddClasses(classes => classes.AssignableTo<IRepository>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }
    }
}
