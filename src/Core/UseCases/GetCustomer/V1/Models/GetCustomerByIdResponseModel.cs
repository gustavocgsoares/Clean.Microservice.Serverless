using System;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models;

namespace Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1.Models
{
    public class GetCustomerByIdResponseModel : IResponseModel
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTimeOffset BirthDate { get; set; }

        public virtual GetCustomerByIdContactModel Contacts { get; set; }
    }
}
