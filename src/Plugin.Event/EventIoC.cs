using Microsoft.Extensions.DependencyInjection;

namespace Clean.Microservice.Serverless.Plugin.Event
{
    /// <summary>
    /// Inversion of Control to event plugin
    /// </summary>
    public static class EventIoC
    {
        private static EventFactory factory = new EventFactory();

        /// <summary>
        /// Add event with IoC.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> injection.</param>
        /// <param name="name">Name to identify the event publisher.</param>
        /// <param name="publisher">Resolved event publisher will be used.</param>
        /// <returns><see cref="IServiceCollection"/> with event plugin injected.</returns>
        public static IServiceCollection AddEventPlugin(
            this IServiceCollection services,
            string name,
            IEventStrategy publisher)
        {
            factory.AddStrategy(name, publisher);
            services.AddSingleton<IEventFactory>(factory);

            return services;
        }
    }
}
