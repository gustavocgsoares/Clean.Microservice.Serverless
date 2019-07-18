namespace Clean.Microservice.Serverless.Plugin.Event.Domain.ActivityMicroservice
{
    /// <summary>
    /// Customer class
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets customer identification
        /// </summary>
        /// <value>
        /// Customer identification
        /// </value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets customer name
        /// </summary>
        /// <value>
        /// Customer name
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Create Customer.
        /// </summary>
        /// <param name="id">Customer id.</param>
        /// <param name="name">Customer name.</param>
        /// <returns>Customer created.</returns>
        public static Customer Builder(string id, string name)
        {
            return new Customer
            {
                Id = id,
                Name = name
            };
        }
    }
}
