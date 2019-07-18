using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC
{
    internal static class MediatRExtensions
    {
        public static void AddMediatr<TUseCaseAssembly>(this IServiceCollection services)
            where TUseCaseAssembly : UseCase
        {
            services.AddMediatR(typeof(TUseCaseAssembly).Assembly);
        }
    }
}
