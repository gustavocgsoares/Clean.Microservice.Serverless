using System.Collections.Generic;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Events;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications
{
    /// <summary>
    /// Domain notification event message.
    /// </summary>
    public sealed class DomainNotification : EventMessage
    {
        /// <summary>
        /// Used to show a default message code.
        /// </summary>
        public const string UnknownMessageCode = "NO_CODE";

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainNotification"/> class.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="messages">List of code and message validations.</param>
        public DomainNotification(string messageType, IDictionary<string, string> messages)
        {
            MessageType = messageType;
            Messages = messages;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainNotification"/> class.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="message">Message validation with default code [NO_CODE].</param>
        public DomainNotification(string messageType, string message)
        {
            MessageType = messageType;
            Messages = new Dictionary<string, string> { { UnknownMessageCode, message } };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainNotification"/> class.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="code">Validation code.</param>
        /// <param name="message">Validation message.</param>
        public DomainNotification(string messageType, string code, string message)
        {
            MessageType = messageType;
            Messages = new Dictionary<string, string> { { code, message } };
        }

        /// <summary>
        /// Gets validation messages.
        /// </summary>
        /// <value>
        /// Validation messages.
        /// </value>
        public IDictionary<string, string> Messages { get; }
    }
}