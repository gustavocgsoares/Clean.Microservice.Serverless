using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Clean.Microservice.Serverless.Plugin.Database.SqlServer
{
    public class SqlServerDatabase : IDatabase
    {
        private string connectionString;

        public virtual IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public virtual void Configure(
            List<DatabaseSettings> databaseSettings,
            string databaseConnectionName)
        {
            var connectionString = databaseSettings?
                .FirstOrDefault(ds => (ds.SettingsKey?
                    .Equals(databaseConnectionName, StringComparison.InvariantCultureIgnoreCase))
                    .GetValueOrDefault())?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                this.connectionString = connectionString;
            }
        }
    }
}
