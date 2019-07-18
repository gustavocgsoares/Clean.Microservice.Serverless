using System.Collections.Generic;
using System.Linq;
using Clean.Microservice.Serverless.SharedKernel.Core.Resources;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models
{
    /// <summary>
    /// Response model for bad request.
    /// </summary>
    public class BadRequestModel : MessageResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestModel"/> class.
        /// </summary>
        /// <param name="code">Response code.</param>
        /// <param name="message">Response message.</param>
        /// <param name="validations">Response validations.</param>
        public BadRequestModel(string code, string message, IReadOnlyCollection<BadRequestValidationModel> validations)
            : base(code, message)
        {
            Validations = validations;
        }

        /// <summary>
        /// Gets response validations.
        /// </summary>
        /// <value>
        /// Response validations.
        /// </value>
        public virtual IReadOnlyCollection<BadRequestValidationModel> Validations { get; private set; }

        /// <summary>
        /// Generate bad request model with default message.
        /// </summary>
        /// <param name="validations">Response validations.</param>
        /// <returns>Bad request model generated.</returns>
        public static BadRequestModel GetDefaultMessage(IReadOnlyCollection<BadRequestValidationModel> validations)
        {
            return new BadRequestModel(
                    HttpMessage.BAD_REQUEST_DEFAULT_CODE,
                    HttpMessage.BAD_REQUEST_DEFAULT_MESSAGE,
                    validations);
        }

        /// <summary>
        /// Generate bad request model with default message.
        /// </summary>
        /// <param name="notifications">Domain notifications.</param>
        /// <returns>Bad request model generated.</returns>
        public static BadRequestModel GetFromNotifications(IEnumerable<DomainNotification> notifications)
        {
            if (notifications?.Count() == 0)
            {
                return null;
            }

            if (notifications?.Count() == 1 && notifications.SelectMany(_ => _.Messages).Count() == 1)
            {
                var mainMessage = notifications
                    .SelectMany(_ => _.Messages)
                    .FirstOrDefault();

                return new BadRequestModel(mainMessage.Key, mainMessage.Value, null);
            }

            var validations = notifications
                    .SelectMany(_ => _.Messages)
                    .Select(m => new BadRequestValidationModel { Code = m.Key, Message = m.Value });

            return new BadRequestModel(
                HttpMessage.BAD_REQUEST_DEFAULT_CODE,
                HttpMessage.BAD_REQUEST_DEFAULT_MESSAGE,
                validations.ToList().AsReadOnly());
        }
    }
}
