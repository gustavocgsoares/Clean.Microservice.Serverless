using System;
using AutoMapper;
using Clean.Microservice.Serverless.Core.Domain.Entities;
using Clean.Microservice.Serverless.Core.Domain.ValueObjects;

namespace Clean.Microservice.Serverless.Core.UseCases.CreateCustomer.V1.Models
{
    public class CreateCustomerProfile : Profile
    {
        public CreateCustomerProfile()
        {
            CreateMap<CreateCustomerRequestModel, CreateCustomerCommand>()
                .ConstructUsing(src => new CreateCustomerCommand(src.Name, src.BirthDate, src.Contacts.Email, src.Contacts.Phone));

            CreateMap<CreateCustomerCommand, Customer>()
                .ForMember(e => e.Contacts, opt => opt.MapFrom(src => Mapper.Map<CustomerContactVO>(src)))
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.CreatedAt, opt => opt.Ignore())
                .ForMember(e => e.Version, opt => opt.Ignore());

            CreateMap<CreateCustomerCommand, CustomerContactVO>();

            CreateMap<Guid, CreateCustomerResult>()
                .ConstructUsing(id => new CreateCustomerResult(id))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<CreateCustomerResult, CreateCustomerResponseModel>();
        }
    }
}
