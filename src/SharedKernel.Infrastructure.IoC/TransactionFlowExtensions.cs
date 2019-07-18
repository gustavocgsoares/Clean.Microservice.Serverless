using Microsoft.Extensions.DependencyInjection;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC
{
    internal static class TransactionFlowExtensions
    {
        public static void AddTransactionFlow<TTransactionFlow>(this IServiceCollection services)
            where TTransactionFlow : class, ITransactionFlow
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ITransactionFlow, TTransactionFlow>();
        }
    }
}
