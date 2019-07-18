using System;
using Clean.Microservice.Serverless.SharedKernel.Core.Domain;

namespace Clean.Microservice.Serverless.Core.Domain.ValueObjects
{
    public class CustomerContactVO
    {
        public string Email { get; private set; }

        public string Phone { get; private set; }
    }
}
