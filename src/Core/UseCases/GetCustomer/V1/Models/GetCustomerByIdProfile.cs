using System;
using AutoMapper;
using Clean.Microservice.Serverless.Core.Domain.Entities;

namespace Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1.Models
{
    public class GetCustomerByIdProfile : Profile
    {
        public GetCustomerByIdProfile()
        {
            CreateMap<Guid, GetCustomerByIdCommand>()
                   .ConstructUsing(id => new GetCustomerByIdCommand(id))
                   .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Customer, GetCustomerByIdResult>()
                .ConstructUsing(c => new GetCustomerByIdResult(c))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<GetCustomerByIdResult, GetCustomerByIdResponseModel>()
                .BeforeMap((src, dst) => Mapper.Map(src.Customer, dst));
        }
    }
}
