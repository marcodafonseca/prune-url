using PruneUrl.Api.Services.Shared.Services;

namespace PruneUrl.Api.UnitTests.Mocks
{
    internal class MockEnvironmentService : IEnvironmentService
    {
        private string keyValue;

        public MockEnvironmentService(string keyValue)
        {
            this.keyValue = keyValue;
        }

        public string GetEnvironmentVariable(string key)
        {
            return keyValue;
        }
    }
}
