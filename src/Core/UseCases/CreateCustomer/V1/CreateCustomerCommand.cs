using System;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Commands;

namespace Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1
{
    public class CreateCustomerCommand : Command<CreateCustomerResult>
    {
        public CreateCustomerCommand(
            string name,
            DateTimeOffset? birthDate,
            string email,
            string phone)
        {
            Name = name;
            BirthDate = birthDate;
            Email = email;
            Phone = phone;
        }

        public string Name { get; }

        public DateTimeOffset? BirthDate { get; }

        public string Email { get; }

        public string Phone { get; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
