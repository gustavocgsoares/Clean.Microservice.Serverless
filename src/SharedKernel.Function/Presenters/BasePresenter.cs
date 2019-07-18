using System.Collections.Generic;
using MediatR;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications;

namespace Clean.Microservice.Serverless.SharedKernel.Function.UseCases.Presenters
{
    /// <summary>
    /// Base presenter.
    /// </summary>
    public class BasePresenter
    {
        private readonly DomainNotificationUseCase notifications;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePresenter"/> class.
        /// </summary>
        /// <param name="notifications">Domain notifications.</param>
        public BasePresenter(INotificationHandler<DomainNotification> notifications)
        {
            this.notifications = (DomainNotificationUseCase)notifications;
        }

        /// <summary>
        /// Gets list of domain notifications.
        /// </summary>
        /// <value>
        /// List of domain notifications.
        /// </value>
        public IEnumerable<DomainNotification> Notifications => notifications.GetNotifications();

        /// <summary>
        /// Check if the function has been successfully executed.
        /// </summary>
        /// <returns>True if the function has been successfully executed. Otherwise, false.</returns>
        public virtual bool IsValidOperation()
        {
            return !notifications.HasNotifications();
        }
    }
}