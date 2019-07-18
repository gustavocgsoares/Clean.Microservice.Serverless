using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Clean.Microservice.Serverless.Function.Unit.Tests.SharedKernel
{
    public static class TestFactory
    {
        public static DefaultHttpRequest CreateHttpRequest()
        {
            return CreateHttpRequest(null, null);
        }

        public static DefaultHttpRequest CreateHttpRequest(object body)
        {
            return CreateHttpRequest(null, body);
        }

        public static DefaultHttpRequest CreateHttpRequest(Dictionary<string, StringValues> query)
        {
            return CreateHttpRequest(query, null);
        }

        public static DefaultHttpRequest CreateHttpRequest(Dictionary<string, StringValues> query, object body)
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            if (query != null)
            {
                request.Query = new QueryCollection(query);
            }

            if (body != null)
            {
                request.Body = CreateBody(body);
            }

            return request;
        }

        private static Stream CreateBody(object body)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            var json = JsonConvert.SerializeObject(body);

            streamWriter.Write(json);
            streamWriter.Flush();

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
