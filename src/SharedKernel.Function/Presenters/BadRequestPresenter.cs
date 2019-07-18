using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models;

namespace Clean.Microservice.Serverless.SharedKernel.Function.UseCases.Presenters
{
    /// <summary>
    /// Presenter for bad request results.
    /// </summary>
    public class BadRequestPresenter : BasePresenter, IPresenter<List<ValidationResult>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestPresenter"/> class.
        /// </summary>
        /// <param name="notifications">Domain notifications.</param>
        public BadRequestPresenter(INotificationHandler<DomainNotification> notifications)
            : base(notifications)
        {
        }

        /// <inheritdoc/>
        public virtual IActionResult ResponseModel { get; private set; }

        /// <inheritdoc/>
        public virtual void Populate(List<ValidationResult> result)
        {
            var validations = result
                .Select(BadRequestValidationModel.ToModel)
                .ToList();

            ResponseModel = new BadRequestObjectResult(BadRequestModel.GetDefaultMessage(validations));
        }

        /// <summary>
        /// Populate response with bad request validation code and message.
        /// </summary>
        /// <param name="code">Bad request validation code.</param>
        /// <param name="message">Bad request validation message.</param>
        public virtual void Populate(string code, string message)
        {
            var validations = new List<BadRequestValidationModel>
            {
                BadRequestValidationModel.ToModel(code, message)
            };

            ResponseModel = new BadRequestObjectResult(BadRequestModel.GetDefaultMessage(validations));
        }
    }
}
