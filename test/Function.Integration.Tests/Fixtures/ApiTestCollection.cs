using Xunit;

namespace Clean.Microservice.Serverless.Function.Integration.Tests.Fixtures
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<ApiTestFixture>
    {
    }
}
