using System;
using System.ComponentModel.DataAnnotations;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models.Attributes
{
    /// <summary>
    /// Attribute to resolve compare properties
    /// </summary>
    public class CompareCustomAttribute : ValidationAttribute
    {
        private readonly string comparisonProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareCustomAttribute"/> class.
        /// </summary>
        /// <param name="comparisonProperty">The property of the object to compare.</param>
        public CompareCustomAttribute(string comparisonProperty)
        {
            if (string.IsNullOrWhiteSpace(comparisonProperty))
            {
                throw new ArgumentException("Property with this name not found", nameof(comparisonProperty));
            }

            this.comparisonProperty = comparisonProperty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareCustomAttribute"/> class.
        /// </summary>
        /// <param name="comparisonProperty">The property of the object to compare.</param>
        /// <param name="errorMessageAccessor">The function that enables access to validation resources</param>
        public CompareCustomAttribute(string comparisonProperty, Func<string> errorMessageAccessor)
            : base(errorMessageAccessor)
        {
            if (string.IsNullOrWhiteSpace(comparisonProperty))
            {
                throw new ArgumentException("Property with this name not found", nameof(comparisonProperty));
            }

            this.comparisonProperty = comparisonProperty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareCustomAttribute"/> class.
        /// </summary>
        /// <param name="comparisonProperty">The property of the object to compare.</param>
        /// <param name="errorMessage">The error message to associate with a validation control</param>
        public CompareCustomAttribute(string comparisonProperty, string errorMessage)
            : base(errorMessage)
        {
            if (string.IsNullOrWhiteSpace(comparisonProperty))
            {
                throw new ArgumentException("Property with this name not found", nameof(comparisonProperty));
            }

            this.comparisonProperty = comparisonProperty;
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <param name="validationContext">Context</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(comparisonProperty);
            if (property == null)
            {
                throw new ArgumentException("Property with this name not found");
            }

            var comparisonValue = (string)property.GetValue(validationContext.ObjectInstance);

            if ((string)value != comparisonValue)
            {
                if (ErrorMessage == null)
                {
                    ErrorMessage = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    ErrorMessageString,
                    validationContext.MemberName,
                    comparisonProperty);
                }

                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
            }

            ErrorMessage = ErrorMessage ?? null;

            return ValidationResult.Success;
        }
    }
}
