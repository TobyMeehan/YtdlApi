namespace YtdlApi.Server.Services;

public interface IYouTubeStreamService
{
    Task<StreamContainer> GetStreamAsync(string video, Format format, Quality quality,
        CancellationToken cancellationToken);
}