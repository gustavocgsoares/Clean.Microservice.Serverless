using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Clean.Microservice.Serverless.Plugin.Logger.Helpers;
using Clean.Microservice.Serverless.SharedKernel.Core.Transaction;

namespace Clean.Microservice.Serverless.Plugin.Logger.Entities
{
    internal class LogApi : LogBase
    {
        public LogApi(ITransactionFlow transactionFlow)
            : base(transactionFlow)
        {
        }

        public override string Type => "Api";

        public virtual DateTimeOffset StartTimestamp { get; set; }

        public virtual DateTimeOffset EndTimestamp { get; set; }

        public virtual decimal Duration => Convert.ToDecimal(Math.Round((EndTimestamp - StartTimestamp).TotalMilliseconds, 2, MidpointRounding.AwayFromZero));

        public virtual LogApiRequest Request { get; set; }

        public virtual LogApiResponse Response { get; set; }

        internal void CreateRequestData(HttpContext context, string requestBody, DateTimeOffset start, IServerlessLogger serverlessLogger)
        {
            Request = new LogApiRequest();

            StartTimestamp = start;
            Request.Body = LoggerHelper.ConvertToJson(requestBody, serverlessLogger);
            Request.Method = context?.Request?.Method;
            Request.Url = $"{context?.Request.Scheme}://{context?.Request.Host}{context?.Request.Path.ToString()}";
            Request.Headers = context?.Request?.Headers?.ToDictionary(h => h.Key, h => h.Value);
        }

        internal void CreateResponseData(HttpContext context, string responseBody, DateTimeOffset end, IServerlessLogger serverlessLogger)
        {
            Response = new LogApiResponse();

            EndTimestamp = end;
            Response.Body = LoggerHelper.ConvertToJson(responseBody, serverlessLogger);
            Response.StatusCode = (context?.Response?.StatusCode).GetValueOrDefault();
            Response.Headers = context?.Response?.Headers?.ToDictionary(h => h.Key, h => h.Value);
        }
    }
}