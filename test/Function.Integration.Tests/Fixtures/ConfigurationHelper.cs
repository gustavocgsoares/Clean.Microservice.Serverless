using Microsoft.Extensions.Configuration;

namespace Clean.Microservice.Serverless.Function.Integration.Tests.Fixtures
{
    public static class ConfigurationHelper
    {
        private static Settings settings;

        public static Settings Settings
        {
            get
            {
                if (settings != null)
                {
                    return settings;
                }

                var configurationRoot = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                settings = new Settings();

                configurationRoot.Bind(settings);

                return settings;
            }
        }
    }
}
