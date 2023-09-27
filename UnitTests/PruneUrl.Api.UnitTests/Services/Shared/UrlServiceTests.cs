using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Dynamo.ORM.Services;
using PruneUrl.Api.Services.Shared.Services;
using PruneUrl.Api.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace PruneUrl.Api.UnitTests.Services.Shared
{
    public class UrlServiceTests
    {
        private readonly ITestOutputHelper output;

        public UrlServiceTests(ITestOutputHelper output) : this()
        {
            this.output = output;
        }

        private readonly AmazonDynamoDBClient client;

        private UrlServiceTests()
        {
            var config = new AmazonDynamoDBConfig
            {
                ServiceURL = "http://localhost:8000/"
            };
            client = new AmazonDynamoDBClient(config);

            var tables = client.ListTablesAsync().Result;

            if (!tables.TableNames.Contains("PrunedUrls"))
                client.CreateTableAsync("PrunedUrls", new List<KeySchemaElement> {
                    new KeySchemaElement("ShortUrl", KeyType.HASH)
                }, new List<AttributeDefinition> {
                    new AttributeDefinition("ShortUrl", ScalarAttributeType.S)
                }, new ProvisionedThroughput(1, 1))
                .Wait();
        }

        [Fact]
        public async void TestUrlService_CreateShortUrl_ExpectSaved()
        {
            var repository = new Repository(client);
            var environmentService = new MockEnvironmentService("");
            var urlService = new UrlService(repository, environmentService);

            var shortUrls = new List<string>();

            for (var i = 0; i < 1000; i++)
            {
                shortUrls.Add(await urlService.ShortenUrl("www.example.com"));
            }

            var duplicates = shortUrls
                .GroupBy(x => x)
                .Where(x => x.Count() > 1);

            var duplicateCount = duplicates.Count();

            Assert.Equal(0, duplicateCount);
        }

        [Fact]
        public async void TestUrlService_GetLongUrl_ExpectSaved()
        {
            var repository = new Repository(client);
            var environmentService = new MockEnvironmentService("");
            var urlService = new UrlService(repository, environmentService);

            var longUrl = "www.example.com";
            var shortUrl = string.Empty;

            var stopWatchShortenUrl = new Stopwatch();
            var stopWatchGetLongUrl = new Stopwatch();

            stopWatchShortenUrl.Start();

            shortUrl = await urlService.ShortenUrl(longUrl);

            stopWatchShortenUrl.Stop();
            stopWatchGetLongUrl.Start();

            var getLongUrl = await urlService.GetLongUrl(shortUrl.Substring(1));

            stopWatchGetLongUrl.Stop();
            
            output.WriteLine($"ShortUrl: {stopWatchShortenUrl.ElapsedMilliseconds}");
            output.WriteLine($"GetLongUrl: {stopWatchGetLongUrl.ElapsedMilliseconds}");

            Assert.Equal(longUrl, getLongUrl);
        }
    }
}
