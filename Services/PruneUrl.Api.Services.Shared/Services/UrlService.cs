using Dynamo.ORM.Services;
using PruneUrl.Api.Services.Shared.Constants;
using PruneUrl.Api.Services.Shared.Models;
using PruneUrl.Api.Services.Shared.Models.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PruneUrl.Api.Services.Shared.Services
{
    public class UrlService : IUrlService
    {
        private readonly IRepository repository;
        private readonly IEnvironmentService environmentService;

        public UrlService(IRepository repository, IEnvironmentService environmentService)
        {
            this.repository = repository;
            this.environmentService = environmentService;
        }

        public UrlService() : this(DependencyFactory.GetRepository(), DependencyFactory.GetEnvironmentService())
        { }

        public async Task<string> GetLongUrl(string shortUrl)
        {
            var prunedUrl = await repository.Get<PrunedUrl>(shortUrl);

            return prunedUrl?.LongUrl;
        }

        public async Task<string> ShortenUrl(string longUrl)
        {
            var shortUrl = await GenerateShortCode();

            var prunedUrl = new PrunedUrl
            {
                LongUrl = longUrl,
                ShortUrl = shortUrl
            };

            await repository.Add(prunedUrl);

            return $"{environmentService.GetEnvironmentVariable(Settings.BasePath)}/{shortUrl}";
        }

        public async Task<IList<BulkPrunedUrl>> BulkShortenUrl(IList<string> longUrls)
        {
            IList<BulkPrunedUrl> responseUrls = new List<BulkPrunedUrl>();

            try
            {
                repository.BeginWriteTransaction();

                foreach (var longUrl in longUrls)
                {
                    responseUrls.Add(new BulkPrunedUrl
                    {
                        PrunedUrl = await ShortenUrl(longUrl),
                        Url = longUrl
                    });
                }

                await repository.CommitWriteTransaction();
            }
            catch (Exception)
            {
                repository.RollbackWriteTransaction();
                throw;
            }

            return responseUrls;
        }

        private async Task<string> GenerateShortCode()
        {
            var shortUrl = string.Empty;
            var isUnique = false;

            do
            {
                shortUrl = GetRandomString(6);

                var entry = await repository.Get<PrunedUrl>(shortUrl);

                if (entry == null)
                    isUnique = true;
            } while (!isUnique);

            return shortUrl;
        }

        private string GetRandomString(int length)
        {
            string charPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-_+";
            var stringBuilder = new StringBuilder();
            var random = new Random();

            while (length > 0)
            {
                stringBuilder.Append(charPool[(int)(random.NextDouble() * charPool.Length)]);
                length--;
            }

            return stringBuilder.ToString();
        }
    }
}
