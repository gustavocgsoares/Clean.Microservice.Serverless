using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace Clean.Microservice.Serverless.Function.Integration.Tests.Fixtures
{
    public class ApiTestFixture : IDisposable
    {
        private readonly Process hostProcess;

        public ApiTestFixture()
        {
            hostProcess = CreateHostProcess();

            if (!hostProcess.Start())
            {
                throw new InvalidOperationException("Could not start Azure Functions host.");
            }
        }

        public int Port { get; } = 7003;

        public HttpClient HttpClient
        {
            get
            {
                return new HttpClient
                {
                    BaseAddress = new Uri($"http://localhost:{Port}")
                };
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!hostProcess.HasExited)
            {
                hostProcess.Kill();
            }

            hostProcess.Dispose();
        }

        private Process CreateHostProcess()
        {
            var functionHostPath = Environment.ExpandEnvironmentVariables(ConfigurationHelper.Settings.FunctionHostPath);
            var functionAppFolder = Path.GetRelativePath(Directory.GetCurrentDirectory(), ConfigurationHelper.Settings.FunctionApplicationPath);

            var process = new Process
            {
                StartInfo =
                {
                    FileName = functionHostPath,
                    Arguments = $"host start -p {Port} --pause-on-erro",
                    WorkingDirectory = functionAppFolder
                },
            };

            //TODO: Parametrizar testes com cosmosdb
            ////process.StartInfo.EnvironmentVariables.Add(Infrastructure.IoC.Helpers.Constants.DatabaseMongoDbCustomer, "");
            ////process.StartInfo.EnvironmentVariables.Add(Infrastructure.IoC.Helpers.Constants.DatabaseSqlServerCustomer, "");
            ////process.StartInfo.EnvironmentVariables.Add(EnvironmentConstants.ConnectionStringCosmosDbCardRequest, "");
            ////process.StartInfo.EnvironmentVariables.Add(Plugin.Database.CosmosDb.Helpers.Constants.PolicyMaxRetryAttemptsOnThrottledRequests, "");
            ////process.StartInfo.EnvironmentVariables.Add(Plugin.Database.CosmosDb.Helpers.Constants.PolicyMaxRetryWaitTimeInSeconds, "");
            ////process.StartInfo.EnvironmentVariables.Add(Plugin.Database.CosmosDb.Helpers.Constants.ConnectionDatabase, "");
            ////process.StartInfo.EnvironmentVariables.Add(Plugin.Database.CosmosDb.Helpers.Constants.ConnectionAuthKey, "");
            ////process.StartInfo.EnvironmentVariables.Add(Plugin.Database.CosmosDb.Helpers.Constants.CollectionOfferThroughput, "");

            return process;
        }
    }
}
