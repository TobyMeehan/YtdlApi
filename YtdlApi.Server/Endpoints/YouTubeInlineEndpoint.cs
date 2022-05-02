using System.Net.Mime;
using FastEndpoints;
using YtdlApi.Server.Exceptions;
using YtdlApi.Server.Services;

namespace YtdlApi.Server.Endpoints;

public class YouTubeInlineEndpoint : Endpoint<YouTubeRequest>
{
    private readonly IYouTubeStreamService _youtube;

    public YouTubeInlineEndpoint(IYouTubeStreamService youtube)
    {
        _youtube = youtube;
    }

    public override void Configure()
    {
        Verbs(Http.GET, Http.POST);
        Routes("/embed", "/embed/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(YouTubeRequest req, CancellationToken ct)
    {
        StreamContainer stream;

        try
        {
            stream = await _youtube.GetStreamAsync(req.Id, req.Format, req.Quality, ct);
        }
        catch (YouTubeNotFoundException)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        catch (YouTubeBadRequestException)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        HttpContext.Response.Headers.Add("Content-Disposition", "inline");

        await SendStreamAsync(stream.Stream, contentType: MimeTypes.GetMimeType($"x.{stream.Type}"),
            enableRangeProcessing: true, cancellation: ct);
    }
}