using FastEndpoints;
using FluentValidation.Results;
using YtdlApi.Server.Exceptions;
using YtdlApi.Server.Services;

namespace YtdlApi.Server.Endpoints;

public class YoutubeEndpoint : Endpoint<YouTubeRequest>
{
    private readonly IYouTubeStreamService _youtube;

    public YoutubeEndpoint(IYouTubeStreamService youtube)
    {
        _youtube = youtube;
    }
    
    public override void Configure()
    {
        Verbs(Http.GET, Http.POST);
        Routes("/", "/{Id}");
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
        
        var filename = $"{req.Filename ?? stream.Name}.{stream.Type}";

        await SendStreamAsync(stream.Stream, filename, contentType: MimeTypes.GetMimeType(filename), enableRangeProcessing: true, cancellation: ct);
    }
}