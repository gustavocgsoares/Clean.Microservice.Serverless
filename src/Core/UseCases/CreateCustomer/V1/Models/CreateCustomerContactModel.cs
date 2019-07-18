using System.ComponentModel.DataAnnotations;
using Clean.Microservice.Serverless.Core.Constants;
using Clean.Microservice.Serverless.Core.Resources;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models;

namespace Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1.Models
{
    public class CreateCustomerContactModel : IModel
    {
        [Display(Name = nameof(EntityFields.CUSTOMER_CONTACT_EMAIL), ResourceType = typeof(EntityFields))]
        [Required(ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_IS_REQUIRED))]
        [MinLength(ValidationConstants.CustomerEmailMinLen, ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_MIN_LENGTH))]
        [MaxLength(ValidationConstants.CustomerEmailMaxLen, ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_MAX_LENGTH))]
        public virtual string Email { get; set; }

        [Display(Name = nameof(EntityFields.CUSTOMER_CONTACT_PHONE), ResourceType = typeof(EntityFields))]
        [Required(ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_IS_REQUIRED))]
        [MinLength(ValidationConstants.CustomerPhoneMinLen, ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_MIN_LENGTH))]
        [MaxLength(ValidationConstants.CustomerPhoneMaxLen, ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_MAX_LENGTH))]
        public virtual string Phone { get; set; }
    }
}
