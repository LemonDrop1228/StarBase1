using System.Net;
using System.Net.Http;
using StarBase1.Services;

namespace StarBase1.HttpMessageHandlers;

public class AdBlockHttpMessageHandler : DelegatingHandler
{
    private readonly AdblockService _adblockService;

    public AdBlockHttpMessageHandler(AdblockService adBlockService)
    {
        _adblockService = adBlockService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_adblockService.ShouldBlock(request.RequestUri?.AbsoluteUri!))
            return new HttpResponseMessage(HttpStatusCode.NoContent); // Blocked

        return await base.SendAsync(request, cancellationToken);  // Not blocked
    }
}
