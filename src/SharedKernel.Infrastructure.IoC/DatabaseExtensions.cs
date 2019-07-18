////namespace Clean.Microservice.Serverless.SharedKernel.Infrastructure.IoC
////{
////    internal static class DatabaseExtensions
////    {
////        ////public static void AddDatabases(this IServiceCollection services)
////        ////{
////        ////    services.AddSqlServer();
////        ////    services.AddCosmosDb();
////        ////    services.AddMongoDb();
////        ////}

////        ////private static void AddSqlServer(this IServiceCollection services)
////        ////{
////        ////    var connectionString = Environment.GetEnvironmentVariable(Constants.DatabaseSqlServerCustomer);

////        ////    var databaseSettings = new List<DatabaseSettings>
////        ////    {
////        ////        DatabaseSettings.Builder(Constants.DatabaseSqlServerCustomer, connectionString)
////        ////    };

////        ////    services.AddTransient<IDatabaseFactory<SqlServerDatabase>>(s =>
////        ////        new DatabaseFactory<SqlServerDatabase>(databaseSettings, Constants.DatabaseSqlServerCustomer));
////        ////}

////        ////private static void AddMongoDb(this IServiceCollection services)
////        ////{
////        ////    MongoDbMapperConfiguration.Setup();
////        ////    var connectionString = Environment.GetEnvironmentVariable(Constants.DatabaseMongoDbCustomer);

////        ////    var databaseSettings = new List<DatabaseSettings>
////        ////    {
////        ////        DatabaseSettings.Builder(Constants.DatabaseMongoDbCustomer, connectionString)
////        ////    };

////        ////    services.AddTransient<IDatabaseFactory<MongoDbDatabase>>(s =>
////        ////        new DatabaseFactory<MongoDbDatabase>(databaseSettings, Constants.DatabaseMongoDbCustomer));
////        ////}

////        ////private static void AddCosmosDb(this IServiceCollection services)
////        ////{
////        ////    var connectionString = Environment.GetEnvironmentVariable(Constants.DatabaseCosmosDbCustomer);
////        ////    var policyMaxRetryAttempts = Environment.GetEnvironmentVariable(CosmosDbHelpers.Constants.PolicyMaxRetryAttemptsOnThrottledRequests);
////        ////    var policyMaxRetryWaitTime = Environment.GetEnvironmentVariable(CosmosDbHelpers.Constants.PolicyMaxRetryWaitTimeInSeconds);
////        ////    var connectionDatabase = Environment.GetEnvironmentVariable(CosmosDbHelpers.Constants.ConnectionDatabase);
////        ////    var connectionAuthKey = Environment.GetEnvironmentVariable(CosmosDbHelpers.Constants.ConnectionAuthKey);
////        ////    var collectionOfferThroughput = Environment.GetEnvironmentVariable(CosmosDbHelpers.Constants.CollectionOfferThroughput);

////        ////    var databaseSettings = new List<DatabaseSettings>
////        ////    {
////        ////        DatabaseSettings
////        ////            .Builder(Constants.DatabaseCosmosDbCustomer, connectionString)
////        ////            .AddParameter(DatabaseSettingsParameter.Builder(CosmosDbHelpers.Constants.PolicyMaxRetryAttemptsOnThrottledRequests, policyMaxRetryAttempts))
////        ////            .AddParameter(DatabaseSettingsParameter.Builder(CosmosDbHelpers.Constants.PolicyMaxRetryWaitTimeInSeconds, policyMaxRetryWaitTime))
////        ////            .AddParameter(DatabaseSettingsParameter.Builder(CosmosDbHelpers.Constants.ConnectionDatabase, connectionDatabase))
////        ////            .AddParameter(DatabaseSettingsParameter.Builder(CosmosDbHelpers.Constants.ConnectionAuthKey, connectionAuthKey))
////        ////            .AddParameter(DatabaseSettingsParameter.Builder(CosmosDbHelpers.Constants.CollectionOfferThroughput, collectionOfferThroughput))
////        ////    };

////        ////    CosmosDbHelpers.CosmosDbConfiguration.Setup(databaseSettings, Constants.DatabaseCosmosDbCustomer);

////        ////    services.AddSingleton<IDatabaseFactory<CosmosDbDatabase>>(s =>
////        ////        new DatabaseFactory<CosmosDbDatabase>(databaseSettings, Constants.DatabaseCosmosDbCustomer));
////        ////}
////    }
////}
