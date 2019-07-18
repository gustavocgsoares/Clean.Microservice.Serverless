using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Clean.Microservice.Serverless.Plugin.Event.Resources;

namespace Clean.Microservice.Serverless.Plugin.Event
{
    /// <summary>
    /// Class to manage event plugin
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not applicable")]
    public class EventFactory : IEventFactory
    {
        private static Dictionary<string, IEventStrategy> strategies =
            new Dictionary<string, IEventStrategy>();

        /// <summary>
        /// Build a event factory with a list of event publisher.
        /// </summary>
        /// <param name="publishers">Resolved event publishers will be used.</param>
        /// <returns>Event factory created.</returns>
        public static IEventFactory Builder(Dictionary<string, IEventStrategy> publishers)
        {
            var factory = new EventFactory();

            factory.AddStrategies(publishers);

            return factory;
        }

        /// <summary>
        /// Build a event factory with an event publisher.
        /// </summary>
        /// <param name="name">Name to identify the event publisher.</param>
        /// <param name="publisher">Resolved event publisher will be used.</param>
        /// <returns>Event factory created.</returns>
        public static IEventFactory Builder(string name, IEventStrategy publisher)
        {
            var factory = new EventFactory();

            factory.AddStrategy(name, publisher);

            return factory;
        }

        /// <summary>
        /// Get event publisher by name.
        /// </summary>
        /// <param name="name">Name of event publisher.</param>
        /// <returns>Event publisher.</returns>
        public IEventStrategy Get(string name)
        {
            return strategies[name];
        }

        internal void AddStrategies(Dictionary<string, IEventStrategy> publishers)
        {
            if (publishers.Count == 0)
            {
                throw new ArgumentException(Messages.PUBLISHER_LIST_IS_EMPTY, nameof(publishers));
            }

            foreach (var item in publishers)
            {
                AddStrategy(item.Key, item.Value);
            }
        }

        internal void AddStrategy(string name, IEventStrategy publisher)
        {
            ValidateParameters(name, publisher);
            strategies.Add(name, publisher);
        }

        private static void ValidateParameters(string name, IEventStrategy publisher)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(string.Format(Messages.REQUIRED_PARAMETER, nameof(name)), nameof(name));
            }

            if (publisher == null)
            {
                throw new ArgumentException(string.Format(Messages.REQUIRED_PARAMETER, nameof(publisher)), nameof(publisher));
            }

            if (strategies.ContainsKey(name))
            {
                throw new ArgumentException(string.Format(Messages.NAME_ALREADY_EXISTS, name), nameof(name));
            }
        }
    }
}
