using Amazon.Lambda.Core;
using PruneUrl.Api.Services.Shared.Services;
using PruneUrl.Api.ShortenUrl.Models.Function;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace PruneUrl.Api.ShortenUrl
{
    public class Function
    {
        private readonly IUrlService urlService;

        public Function(IUrlService urlService)
        {
            this.urlService = urlService;
        }

        public Function() : this(DependencyFactory.GetUrlService())
        { }

        public async Task<Response> FunctionHandler(Request request, ILambdaContext context)
        {
            var shortUrl = await urlService.ShortenUrl(request.Url);

            return new Response
            {
                PrunedUrl = shortUrl
            };
        }
    }
}
