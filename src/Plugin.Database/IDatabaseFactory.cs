namespace Clean.Microservice.Serverless.Plugin.Database
{
    public interface IDatabaseFactory<TDatabase>
        where TDatabase : IDatabase
    {
        TDatabase Create();
    }
}