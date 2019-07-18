using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC
{
    internal static class DomainExtensions
    {
        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationUseCase>();
        }
    }
}
