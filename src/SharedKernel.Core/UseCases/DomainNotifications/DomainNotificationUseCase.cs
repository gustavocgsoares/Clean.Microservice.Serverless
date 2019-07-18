using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications
{
    /// <summary>
    /// Domain notification use case used to notify system errors.
    /// </summary>
    public class DomainNotificationUseCase : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> notifications;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainNotificationUseCase"/> class.
        /// </summary>
        public DomainNotificationUseCase()
        {
            notifications = new List<DomainNotification>();
        }

        /// <summary>
        /// Notify system error.
        /// </summary>
        /// <param name="message">Domain notification message.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Task result.</returns>
        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            notifications.Add(message);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get notifications.
        /// </summary>
        /// <returns>List of domain notifications.</returns>
        public virtual List<DomainNotification> GetNotifications()
        {
            return notifications;
        }

        /// <summary>
        /// Check if use case has domain notifications.
        /// </summary>
        /// <returns>True if has domain notifications.</returns>
        public virtual bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        /// <summary>
        /// Clear the list of notifications.
        /// </summary>
        public void Dispose()
        {
            notifications = new List<DomainNotification>();
        }
    }
}
