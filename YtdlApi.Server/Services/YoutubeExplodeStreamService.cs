using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YtdlApi.Server.Services;

public class YoutubeExplodeStreamService : IYouTubeStreamService
{
    private readonly YoutubeClient _youtube;

    public YoutubeExplodeStreamService(YoutubeClient youtube)
    {
        _youtube = youtube;
    }
    
    public async Task<Stream> GetStreamAsync(string videoId, Format format, Quality quality, string filename)
    {
        var video = await _youtube.Videos.GetAsync(videoId);

        var manifest = await _youtube.Videos.Streams.GetManifestAsync(video.Id);

        var streamInfo = GetBestStream(manifest, format, quality);

        return await _youtube.Videos.Streams.GetAsync(streamInfo);
    }

    private static IStreamInfo GetBestStream(StreamManifest manifest, Format format, Quality quality)
    {
        return format switch
        {
            Format.Video => GetStreamByQuality(manifest.GetVideoStreams(), quality),
            Format.VideoOnly => GetStreamByQuality(manifest.GetVideoOnlyStreams(), quality),
            Format.Audio => GetStreamByQuality(manifest.GetAudioStreams(), quality),
            Format.AudioOnly => GetStreamByQuality(manifest.GetAudioOnlyStreams(), quality),
            _ => GetStreamByQuality(manifest.GetMuxedStreams(), quality)
        };
    }

    private static IStreamInfo GetStreamByQuality(IEnumerable<IVideoStreamInfo> streams, Quality quality)
    {
        return quality switch
        {
            Quality.Bitrate => streams.GetWithHighestBitrate(),
            _ => streams.GetWithHighestVideoQuality()
        };
    }

    private static IStreamInfo GetStreamByQuality(IEnumerable<IAudioStreamInfo> streams, Quality quality)
    {
        return quality switch
        {
            Quality.Video => throw new ArgumentOutOfRangeException(nameof(quality), quality,
                "Cannot get highest video quality for audio streams."),
            _ => streams.GetWithHighestBitrate()
        };
    }

    private static IStreamInfo GetStreamByQuality(IEnumerable<MuxedStreamInfo> streams, Quality quality)
    {
        return quality switch
        {
            Quality.Bitrate => streams.GetWithHighestBitrate(),
            _ => streams.GetWithHighestVideoQuality()
        };
    }
}