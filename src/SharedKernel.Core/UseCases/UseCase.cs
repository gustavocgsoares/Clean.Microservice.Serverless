using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Commands;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases
{
    /// <summary>
    /// Base use case class.
    /// </summary>
    public class UseCase
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseCase"/> class.
        /// </summary>
        /// <param name="mediator">See <see cref="IMediator"/>.</param>
        /// <param name="logger">See <see cref="ILogger"/>.</param>
        public UseCase(IMediator mediator, ILogger logger)
        {
            this.mediator = mediator;
            Logger = logger;
        }

        /// <summary>
        /// Gets logger.
        /// </summary>
        /// <value>
        /// Logger.
        /// </value>
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Gets notify dictionary.
        /// </summary>
        /// <value>
        /// Notify dictionary.
        /// </value>
        public Dictionary<string, string> Notify { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Notify validation errors used when <see cref="ICommand"/> is invalid.
        /// </summary>
        /// <param name="message">Message to be validated.</param>
        public void NotifyValidationErrors(ICommand message)
        {
            if (!(message?.ValidationResult?.Errors?.Any()).GetValueOrDefault())
            {
                return;
            }

            Notify = message?
                .ValidationResult?
                .Errors?
                .ToDictionary(t => t.ErrorCode, t => t.ErrorMessage);

            mediator.Publish(new DomainNotification(message?.MessageType, Notify));
            Logger.LogInformation("Invalid parameters", message, Notify);
        }

        /// <summary>
        /// Notify system error.
        /// </summary>
        /// <param name="error">Error data.</param>
        public void NotifyError(Error error)
        {
            if (error == null)
            {
                return;
            }

            Notify.Add(error.Code ?? DomainNotification.UnknownMessageCode, error.Message);
            mediator.Publish(new DomainNotification(error.Code ?? error?.Exception?.GetType().Name, Notify));

            if (error.ErrorLevel == ErrorLevel.Business)
            {
                Logger.LogInformation("Error notification", error);
            }
            else
            {
                Logger.LogError("Error notification", error);
            }
        }

        /// <summary>
        /// Notify system error with code and message.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <param name="message">error message.</param>
        public void NotifyError(string code, string message)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(message))
            {
                return;
            }

            Notify.Add(code, message);
            mediator.Publish(new DomainNotification(code, Notify));
            Logger.LogInformation("Error notification", code, message);
        }
    }
}