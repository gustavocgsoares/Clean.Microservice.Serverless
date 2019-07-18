using Clean.Microservice.Serverless.Function;
using Clean.Microservice.Serverless.Infrastructure.IoC;
using Clean.Microservice.Serverless.SharedKernel.Function.UseCases.Presenters;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Clean.Microservice.Serverless.Function
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder) =>
            builder.AddDependencyInjection(ConfigureServices);

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices<BadRequestPresenter, UseCases.GetCustomerById.V1.Presenter>();
        }
    }
}
