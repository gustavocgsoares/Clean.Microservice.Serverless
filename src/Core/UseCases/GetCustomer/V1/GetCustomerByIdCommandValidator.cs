using System;
using Clean.Microservice.Serverless.Core.Resources;
using FluentValidation;

namespace Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1
{
    public sealed class GetCustomerByIdCommandValidator : AbstractValidator<GetCustomerByIdCommand>
    {
        public GetCustomerByIdCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEqual(Guid.Empty)
                .WithErrorCode(EntityFields.CUSTOMER_ID)
                .WithMessage(string.Format(ValidationFields.FIELD_IS_REQUIRED, EntityFields.CUSTOMER_ID));
        }
    }
}
