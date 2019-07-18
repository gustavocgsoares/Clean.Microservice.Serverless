using System;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Messages
{
    /// <summary>
    /// Base class for message types.
    /// </summary>
    public abstract class Message : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        protected Message()
        {
            MessageType = GetType().Name;
        }

        /// <inheritdoc/>
        public string MessageType { get; protected set; }

        /// <inheritdoc/>
        public Guid AggregateId { get; protected set; }
    }
}