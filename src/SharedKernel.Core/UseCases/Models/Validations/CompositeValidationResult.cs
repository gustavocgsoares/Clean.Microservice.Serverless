using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models.Validations
{
    /// <summary>
    /// validation result for nested objects.
    /// </summary>
    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        public CompositeValidationResult(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <param name="memberNames">Member names.</param>
        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames)
            : base(errorMessage, memberNames)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidationResult"/> class.
        /// </summary>
        /// <param name="validationResult">Validation result.</param>
        protected CompositeValidationResult(ValidationResult validationResult)
            : base(validationResult)
        {
        }

        /// <summary>
        /// Gets validation results.
        /// </summary>
        /// <value>
        /// Validation results.
        /// </value>
        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return _results;
            }
        }

        /// <summary>
        /// Add validation result.
        /// </summary>
        /// <param name="validationResult">Validation result.</param>
        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }
    }
}
