using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clean.Microservice.Serverless.SharedKernel.Core.Resources;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models.Validations;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models.Attributes
{
    /// <summary>
    /// Validate nested object attribute.
    /// </summary>
    public class ValidateNestedObjectAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <param name="validationContext">Context.</param>
        /// <returns>True if the specified value is valid; otherwise, false.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Count != 0)
            {
                var message = string.Format(HttpMessage.COMPOSITE_VALIDATION_FAIL_MESSAGE, validationContext.DisplayName);
                var compositeResults = new CompositeValidationResult(message);

                results.ForEach(compositeResults.AddResult);

                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }
}
