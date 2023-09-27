using PruneUrl.Api.Services.Shared.Services;

namespace PruneUrl.Api.ShortenUrl
{
    internal static class DependencyFactory
    {
        internal static IUrlService GetUrlService() => new UrlService();
    }
}
