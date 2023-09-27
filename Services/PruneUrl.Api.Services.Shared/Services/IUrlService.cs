using PruneUrl.Api.Services.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruneUrl.Api.Services.Shared.Services
{
    public interface IUrlService
    {
        Task<string> ShortenUrl(string shortUrl);
        Task<IList<BulkPrunedUrl>> BulkShortenUrl(IList<string> shortenUrls);
        Task<string> GetLongUrl(string shortUrl);
    }
}
