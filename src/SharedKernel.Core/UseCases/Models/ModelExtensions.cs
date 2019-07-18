using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models.Validations;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models
{
    /// <summary>
    /// Extensions for models.
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Get model validations from data annotations.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <param name="model">Model to be validated.</param>
        /// <returns>Model validations.</returns>
        public static List<ValidationResult> GetValidations<TModel>(this TModel model)
            where TModel : IModel, new()
        {
            var validations = new List<ValidationResult>();
            var validationsResult = new List<ValidationResult>();

            if (model == null)
            {
                model = new TModel();
            }

            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validations, true);

            validations.ForEach(v =>
            {
                if (v is CompositeValidationResult)
                {
                    validationsResult.AddRange(((CompositeValidationResult)v).Results);
                }
                else
                {
                    validationsResult.Add(v);
                }
            });

            return validationsResult;
        }
    }
}
