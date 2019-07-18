namespace Clean.Microservice.Serverless.Plugin.Event
{
    /// <summary>
    /// Event factory.
    /// </summary>
    public interface IEventFactory
    {
        /// <summary>
        /// Get event publisher by name.
        /// </summary>
        /// <param name="name">Name of event publisher.</param>
        /// <returns>Event publisher.</returns>
        IEventStrategy Get(string name);
    }
}
