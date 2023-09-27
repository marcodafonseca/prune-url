using Amazon.Lambda.Core;
using PruneUrl.Api.BulkShortenUrl.Models.Function;
using PruneUrl.Api.Services.Shared.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace PruneUrl.Api.BulkShortenUrl
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

        public async Task<IList<Response>> FunctionHandler(IList<Request> requestList, ILambdaContext context)
        {
            var responseList = new List<Response>();

            foreach (var request in requestList)
            {
                var response = new Response
                {
                    Url = request.Url,
                    PrunedUrl = await urlService.ShortenUrl(request.Url)
                };

                responseList.Add(response);
            }

            return responseList;
        }
    }
}
