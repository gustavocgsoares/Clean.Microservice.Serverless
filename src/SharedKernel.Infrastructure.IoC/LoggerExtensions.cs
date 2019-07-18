using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Clean.Microservice.Serverless.Plugin.Logger;
using Clean.Microservice.Serverless.Plugin.Logger.Entities;
using Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC.Resources;

namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC
{
    internal static class LoggerExtensions
    {
        public static void AddLoggers(this IServiceCollection services, string appInsightsInstrumentationKey, List<string> loggerBlacklist = null)
        {
            if (string.IsNullOrWhiteSpace(appInsightsInstrumentationKey))
            {
                throw new ArgumentException(string.Format(Messages.REQUIRED_PARAMETER, nameof(appInsightsInstrumentationKey)), nameof(appInsightsInstrumentationKey));
            }

            services.AddLogging(builder =>
            {
                builder.AddApplicationInsights(appInsightsInstrumentationKey);
            });

            services.AddScoped<ILogger, ServerlessLogger>();
            services.AddScoped<IServerlessLogger, ServerlessLogger>();
            AddBlacklist(services, loggerBlacklist);
        }

        private static void AddBlacklist(IServiceCollection services, List<string> loggerBlacklist)
        {
            var blacklist = new LogBlacklist();

            if ((loggerBlacklist?.Any()).GetValueOrDefault())
            {
                blacklist.AddRange(loggerBlacklist);
            }

            services.AddSingleton(lb => blacklist);
        }
    }
}
