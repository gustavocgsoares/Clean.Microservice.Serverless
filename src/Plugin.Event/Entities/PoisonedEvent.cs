using System;
using System.Collections.Generic;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;

namespace Clean.Microservice.Serverless.Plugin.Event.Entities
{
    /// <summary>
    /// Entity to manage a poisoned event.
    /// </summary>
    public class PoisonedEvent : Entity<Guid>
    {
        /// <summary>
        /// Gets or sets poisoned event id.
        /// </summary>
        /// <value>
        /// Poisoned event id.
        /// </value>
        public override Guid Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// Gets or sets connection string for event strategy to republish a poisoned event.
        /// </summary>
        /// <value>
        /// Connection string for event strategy to republish a poisoned event.
        /// </value>
        public virtual string EventStrategyConnection { get; set; }

        /// <summary>
        /// Gets or sets microservice name to identify the owner of the poisoned event.
        /// </summary>
        /// <value>
        /// Microservice name to identify the owner of the poisoned event.
        /// </value>
        public virtual string Microservice { get; set; }

        /// <summary>
        /// Gets or sets event or queue name to republish event.
        /// </summary>
        /// <value>
        /// Event or queue name to republish event.
        /// </value>
        public virtual string EventName { get; set; }

        /// <summary>
        /// Gets or sets date and time of fail.
        /// </summary>
        /// <value>
        /// Date and time of fail.
        /// </value>
        public virtual DateTime FailAt { get; set; }

        /// <summary>
        /// Gets or sets correlation id.
        /// </summary>
        /// <value>
        /// Correlation id.
        /// </value>
        public virtual string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets event data to be republished.
        /// </summary>
        /// <value>
        /// Event data to be republished.
        /// </value>
        public virtual object EventData { get; set; }

        /// <summary>
        /// Gets or sets the custom properties from event.
        /// </summary>
        /// <value>
        /// The custom properties from event.
        /// </value>
        public virtual IDictionary<string, object> CustomProperties { get; set; }

        /// <summary>
        /// Gets or sets wathever additional info about the event.
        /// </summary>
        /// <value>
        /// Wathever additional info about the event.
        /// </value>
        public virtual object AdditionalInfo { get; set; }

        /// <summary>
        /// Gets or sets fail data like error, message or others.
        /// </summary>
        /// <value>
        /// Fail data like error, message or others.
        /// </value>
        public virtual object FailData { get; set; }

        /// <summary>
        /// Gets or sets retries quantity before publish the poisoned event.
        /// </summary>
        /// <value>
        /// Retries quantity before publish the poisoned event.
        /// </value>
        public virtual int Retries { get; set; }
    }
}
