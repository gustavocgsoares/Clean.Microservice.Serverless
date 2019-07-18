using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Clean.Microservice.Serverless.Plugin.Logger.Entities;

namespace Clean.Microservice.Serverless.Plugin.Logger
{
    /// <summary>
    /// Inversion of Control to serverless logger plugin.
    /// </summary>
    public static class ServerlessLoggerIoC
    {
        /// <summary>
        /// Add serverless logger with IoC.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> injection.</param>
        /// <param name="loggerBlacklist">Logger blacklist with sensitive fields to be masked.</param>
        public static void AddServerlessLoggerPlugin(this IServiceCollection services, params string[] loggerBlacklist)
        {
            services.AddServerlessLoggerPlugin(loggerBlacklist?.ToList());
        }

        /// <summary>
        /// Add serverless logger with IoC.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> injection.</param>
        /// <param name="loggerBlacklist">Logger blacklist with sensitive fields to be masked.</param>
        public static void AddServerlessLoggerPlugin(this IServiceCollection services, List<string> loggerBlacklist = null)
        {
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