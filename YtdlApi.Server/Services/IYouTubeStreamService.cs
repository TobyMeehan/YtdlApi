namespace YtdlApi.Server.Services;

public interface IYouTubeStreamService
{
    Task<Stream> GetStreamAsync(string video, Format format, Quality quality, string filename);
}