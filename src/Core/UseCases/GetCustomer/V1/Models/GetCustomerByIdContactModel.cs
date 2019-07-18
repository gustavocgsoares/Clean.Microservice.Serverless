using System.ComponentModel.DataAnnotations;
using Clean.Microservice.Serverless.Core.Constants;
using Clean.Microservice.Serverless.Core.Resources;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models;

namespace Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1.Models
{
    public class GetCustomerByIdContactModel : IModel
    {
        public virtual string Email { get; set; }

        public virtual string Phone { get; set; }
    }
}
