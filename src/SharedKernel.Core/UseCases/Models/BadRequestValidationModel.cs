using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models
{
    /// <summary>
    /// Bad request validation model.
    /// </summary>
    public class BadRequestValidationModel : IModel
    {
        /// <summary>
        /// Gets or sets validation code.
        /// </summary>
        /// <value>
        /// Response code.
        /// </value>
        public virtual string Code { get; set; }

        /// <summary>
        /// Gets or sets validation code.
        /// </summary>
        /// <value>
        /// Response code.
        /// </value>
        public virtual string Message { get; set; }

        /// <summary>
        /// Convert <see cref="ValidationResult"/> to <see cref="BadRequestValidationModel"/>.
        /// </summary>
        /// <param name="validationResult">Validation result.</param>
        /// <returns>Bad request validation model with validation result converted.</returns>
        public static BadRequestValidationModel ToModel(ValidationResult validationResult)
        {
            return new BadRequestValidationModel
            {
                Code = validationResult.MemberNames.First(),
                Message = validationResult.ErrorMessage
            };
        }

        /// <summary>
        /// Convert code and message to <see cref="BadRequestValidationModel"/>.
        /// </summary>
        /// <param name="code">Validation code.</param>
        /// <param name="message">Validation message.</param>
        /// <returns>Bad request validation model with validation result converted.</returns>
        public static BadRequestValidationModel ToModel(string code, string message)
        {
            return new BadRequestValidationModel
            {
                Code = code,
                Message = message,
            };
        }
    }
}