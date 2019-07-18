using System.Collections.Generic;

namespace Clean.Microservice.Serverless.Plugin.Database
{
    public class DatabaseFactory<TDatabase> : IDatabaseFactory<TDatabase>
        where TDatabase : IDatabase, new()
    {
        private readonly List<DatabaseSettings> databaseSettings;
        private readonly string databaseConnectionName;
        private TDatabase database;

        public DatabaseFactory(
            List<DatabaseSettings> databaseSettings,
            string databaseConnectionName)
        {
            this.databaseSettings = databaseSettings;
            this.databaseConnectionName = databaseConnectionName;
        }

        public virtual TDatabase Create()
        {
            var database = new TDatabase();
            database.Configure(databaseSettings, databaseConnectionName);
            return this.database = database;
        }
    }
}
