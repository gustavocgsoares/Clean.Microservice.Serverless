using System;
using FluentValidation.Results;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Messages;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Commands
{
    /// <summary>
    /// Command message.
    /// </summary>
    public interface ICommand : IMessage
    {
        /// <summary>
        /// Gets command timestamp.
        /// </summary>
        /// <value>
        /// Timestamp.
        /// </value>
        DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Gets validation result.
        /// </summary>
        /// <value>
        /// Validation result.
        /// </value>
        ValidationResult ValidationResult { get; }

        /// <summary>
        /// Check if command is valid. When not, the <see cref="ValidationResult"/> will be filled.
        /// </summary>
        /// <returns>True if command is valid. Otherwise false.</returns>
        bool IsValid();
    }
}