using System.Collections.Generic;

namespace Clean.Microservice.Serverless.Plugin.Database
{
    public interface IDatabase
    {
        void Configure(List<DatabaseSettings> databaseSettings, string databaseConnectionName);
    }
}