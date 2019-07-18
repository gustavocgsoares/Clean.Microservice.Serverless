using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Presenters;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC
{
    internal static class PresenterExtensions
    {
        public static void AddPresenters<TSharedKernelPresenterAssembly, TPresenterAssembly>(this IServiceCollection services)
            where TSharedKernelPresenterAssembly : class, IPresenter
            where TPresenterAssembly : class, IPresenter
        {
            services.Scan(config =>
            {
                config.FromAssemblyOf<TPresenterAssembly>()
                    .AddClasses(classes => classes.AssignableTo<IPresenter>())
                    .AsSelf()
                    .WithScopedLifetime();

                config.FromAssemblyOf<TSharedKernelPresenterAssembly>()
                    .AddClasses(classes => classes.AssignableTo<IPresenter>())
                    .AsSelf()
                    .WithScopedLifetime();
            });
        }
    }
}
