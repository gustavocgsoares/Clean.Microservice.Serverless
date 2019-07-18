using AutoMapper;
using Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1;
using Clean.Microservice.Serverless.Core.UseCases.GetCustomerById.V1.Models;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.DomainNotifications;
using Clean.Microservice.Serverless.SharedKernel.Core.UseCases.Models;
using Clean.Microservice.Serverless.SharedKernel.Function.UseCases.Presenters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Microservice.Serverless.Function.UseCases.GetCustomerById.V1
{
    public class Presenter : BasePresenter, IPresenter<GetCustomerByIdResult>
    {
        private readonly IMapper mapper;

        public Presenter(INotificationHandler<DomainNotification> notifications, IMapper mapper)
            : base(notifications)
        {
            this.mapper = mapper;
        }

        public virtual IActionResult ResponseModel { get; private set; }

        public virtual void Populate(GetCustomerByIdResult result)
        {
            if (IsValidOperation())
            {
                var response = mapper.Map<GetCustomerByIdResponseModel>(result);
                ResponseModel = new OkObjectResult(response);
                return;
            }

            ResponseModel = new BadRequestObjectResult(BadRequestModel.GetFromNotifications(Notifications));
        }
    }
}
