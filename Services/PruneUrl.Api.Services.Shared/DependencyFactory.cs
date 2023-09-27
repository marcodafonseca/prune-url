using System;
using Amazon.DynamoDBv2;
using Dynamo.ORM.Services;
using PruneUrl.Api.Services.Shared.Services;

namespace PruneUrl.Api.Services.Shared
{
    internal static class DependencyFactory
    {
        internal static IRepository GetRepository()
        {
            var endpoint = Amazon.RegionEndpoint.EUWest1;
            var client = new AmazonDynamoDBClient(endpoint);

            return new Repository(client);
        }

        internal static IEnvironmentService GetEnvironmentService() => new EnvironmentService();
    }
}
