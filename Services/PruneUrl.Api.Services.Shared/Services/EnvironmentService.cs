using System;

namespace PruneUrl.Api.Services.Shared.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
