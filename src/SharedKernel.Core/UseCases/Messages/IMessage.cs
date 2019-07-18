using System;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Messages
{
    /// <summary>
    /// Interface for message types.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets message type.
        /// </summary>
        /// <value>
        /// Message type.
        /// </value>
        string MessageType { get; }

        /// <summary>
        /// Gets aggregate id.
        /// </summary>
        /// <value>
        /// Aggregate id.
        /// </value>
        Guid AggregateId { get; }
    }
}