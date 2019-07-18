using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Clean.Microservice.Serverless.Plugin.Logger.Entities
{
    internal class LogApiResponse
    {
        public virtual object Body { get; set; }

        public virtual int StatusCode { get; set; }

        public virtual Dictionary<string, StringValues> Headers { get; set; }
    }
}
