using System;
using Clean.Microservice.Serverless.Core.Domain.ValueObjects;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;

namespace Clean.Microservice.Serverless.Core.Domain.Entities
{
    public class Customer : Entity<Guid>, IAggregateRoot
    {
        public string Name { get; private set; }

        public DateTimeOffset BirthDate { get; private set; }

        public CustomerContactVO Contacts { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; } = DateTime.UtcNow;

        public static Customer Builder(Customer customer, CustomerContactVO contact)
        {
            customer.Contacts = contact;

            return customer;
        }
    }
}
