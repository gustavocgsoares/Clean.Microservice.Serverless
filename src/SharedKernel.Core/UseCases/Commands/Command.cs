using System;
using FluentValidation.Results;
using MediatR;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Messages;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Results;

namespace Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Commands
{
    /// <summary>
    /// Base command class.
    /// </summary>
    /// <typeparam name="TResult">Type of result.</typeparam>
    public abstract class Command<TResult> : Message, IRequest<TResult>, ICommand
        where TResult : IResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command{TResult}"/> class.
        /// </summary>
        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets timestamp with UtcNow value.
        /// </summary>
        /// <value>
        /// Timestamp with UtcNow value.
        /// </value>
        public virtual DateTimeOffset Timestamp { get; private set; }

        /// <inheritdoc/>
        public virtual ValidationResult ValidationResult { get; protected set; }

        /// <inheritdoc/>
        public abstract bool IsValid();
    }
}