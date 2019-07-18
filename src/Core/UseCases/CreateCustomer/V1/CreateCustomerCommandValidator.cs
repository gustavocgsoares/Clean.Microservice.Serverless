using Clean.Microservice.Serverless.Core.Resources;
using FluentValidation;

namespace Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .WithErrorCode(EntityFields.CUSTOMER_NAME)
                .WithMessage(string.Format(ValidationFields.FIELD_IS_REQUIRED, EntityFields.CUSTOMER_NAME));

            RuleFor(r => r.Email)
                .NotEmpty()
                .WithErrorCode(EntityFields.CUSTOMER_CONTACT_EMAIL)
                .WithMessage(string.Format(ValidationFields.FIELD_IS_REQUIRED, EntityFields.CUSTOMER_CONTACT_EMAIL));

            RuleFor(r => r.Name)
                .NotEmpty()
                .WithErrorCode(EntityFields.CUSTOMER_CONTACT_PHONE)
                .WithMessage(string.Format(ValidationFields.FIELD_IS_REQUIRED, EntityFields.CUSTOMER_CONTACT_PHONE));
        }
    }
}
