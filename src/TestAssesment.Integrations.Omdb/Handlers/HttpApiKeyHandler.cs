using Microsoft.Extensions.Options;
using System.Web;
using TestAssesment.Integrations.Omdb.Configurations;

namespace TestAssesment.Integrations.Omdb.Handlers;

public class HttpApiKeyHandler(IOptions<OmdbConfiguration> omdbOptions) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var uriBuilder = new UriBuilder(request.RequestUri!);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["apikey"] = omdbOptions.Value.ApiKey;
        uriBuilder.Query = query.ToString();
        request.RequestUri = uriBuilder.Uri;

        return base.SendAsync(request, cancellationToken);
    }
}