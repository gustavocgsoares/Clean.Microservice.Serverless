using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Clean.Microservice.Serverless.Infrastructure
{
    public static class CommandQuery
    {
        public static string CreateCustomer => GetQuery("Clean.Microservice.Serverless.Infrastructure.UseCases.CreateCustomer.V1.CreateCustomer.sql");

        public static string GetCustomerById => GetQuery("Clean.Microservice.Serverless.Infrastructure.UseCases.GetCustomerById.V1.GetCustomerById.sql");

        private static string GetQuery(string filename = null, [CallerMemberName]string propertyName = null)
        {
            var stream = typeof(CommandQuery).Assembly.GetManifestResourceStream(filename);

            if (stream == null)
            {
                throw new Exception($"The file {propertyName}.sql was not found in Clean.Microservice.Serverless.Infrastructure. Don't forget to set Build Action = 'Embedded resource' in the file properties.");
            }

            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
