using System;
using System.Collections.Generic;
using System.Linq;

namespace Clean.Microservice.Serverless.Plugin.Database
{
    public class DatabaseSettings
    {
        public DatabaseSettings()
        {
            Parameters = new List<DatabaseSettingsParameter>();
        }

        public virtual string SettingsKey { get; set; }

        public virtual string ConnectionString { get; set; }

        public virtual List<DatabaseSettingsParameter> Parameters { get; private set; }

        public static DatabaseSettings Builder(string settingsKey, string connectionString, List<DatabaseSettingsParameter> parameters = null)
        {
            return new DatabaseSettings
            {
                SettingsKey = settingsKey,
                ConnectionString = connectionString,
                Parameters = parameters ?? new List<DatabaseSettingsParameter>()
            };
        }

        public DatabaseSettings AddParameter(DatabaseSettingsParameter parameter)
        {
            if (parameter != null)
            {
                Parameters.Add(parameter);
            }

            return this;
        }

        public string GetParameterValue(string code)
        {
            return Parameters
                .FirstOrDefault(p => p.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase))?
                .Value;
        }
    }
}