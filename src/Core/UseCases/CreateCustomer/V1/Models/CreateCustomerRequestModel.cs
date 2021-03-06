﻿// <auto-generated/>
using System;
using System.ComponentModel.DataAnnotations;
using Clean.Microservice.Serverless.Core.Constants;
using Clean.Microservice.Serverless.Core.Resources;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models.Attributes;

namespace Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1.Models
{
    /// <summary>
    /// Define create customer request data.
    /// </summary>
    public class CreateCustomerRequestModel : IRequestModel
    {
        /// <summary>
        /// Customer name.
        /// </summary>
        [Display(Name = nameof(EntityFields.CUSTOMER_NAME), ResourceType = typeof(EntityFields))]
        [Required(ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_IS_REQUIRED))]
        [MinLength(ValidationConstants.CustomerNameMinLen, ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_MIN_LENGTH))]
        [MaxLength(ValidationConstants.CustomerNameMaxLen, ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_MAX_LENGTH))]
        public virtual string Name { get; set; }

        /// <summary>
        /// Customer birth date.
        /// </summary>
        [Display(Name = nameof(EntityFields.CUSTOMER_BIRTH_DATE), ResourceType = typeof(EntityFields))]
        [Required(ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_IS_REQUIRED))]
        public virtual DateTimeOffset? BirthDate { get; set; }

        /// <summary>
        /// Customer contacts.
        /// </summary>
        [ValidateNestedObject]
        [Display(Name = nameof(EntityFields.CUSTOMER_CONTACT), ResourceType = typeof(EntityFields))]
        [Required(ErrorMessageResourceType = typeof(ValidationFields), ErrorMessageResourceName = nameof(ValidationFields.FIELD_IS_REQUIRED))]
        public virtual CreateCustomerContactModel Contacts { get; set; }
    }
}