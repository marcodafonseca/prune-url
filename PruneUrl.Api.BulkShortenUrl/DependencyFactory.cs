using PruneUrl.Api.Services.Shared.Services;

namespace PruneUrl.Api.BulkShortenUrl
{
    public static class DependencyFactory
    {
        internal static IUrlService GetUrlService() => new UrlService();
    }
}
