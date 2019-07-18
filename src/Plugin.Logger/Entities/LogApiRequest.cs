using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Clean.Microservice.Serverless.Plugin.Logger.Entities
{
    internal class LogApiRequest
    {
        public virtual string Url { get; set; }

        public virtual object Body { get; set; }

        public virtual string Method { get; set; }

        public virtual Dictionary<string, StringValues> Headers { get; set; }
    }
}
