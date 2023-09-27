using Amazon.DynamoDBv2.DataModel;
using Dynamo.ORM.Models;
using PruneUrl.Api.Services.Shared.Constants;
using System;

namespace PruneUrl.Api.Services.Shared.Models.Repository
{
    [DynamoDBTable(Pages.PrunedUrls)]
    public class PrunedUrl : Base
    {
        [DynamoDBHashKey]
        public string ShortUrl { get; set; }
        public string LongUrl { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
