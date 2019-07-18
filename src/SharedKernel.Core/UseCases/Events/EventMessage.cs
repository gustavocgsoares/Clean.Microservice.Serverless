using System;
using MediatR;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Messages;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Events
{
    /// <summary>
    /// Event message.
    /// </summary>
    public abstract class EventMessage : Message, INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessage"/> class.
        /// </summary>
        protected EventMessage()
        {
            Timestamp = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Gets timestamp with UtcNow value.
        /// </summary>
        /// <value>
        /// Timestamp with UtcNow value.
        /// </value>
        public DateTimeOffset Timestamp { get; private set; }
    }
}