namespace YtdlApi.Server.Endpoints;

public class YouTubeRequest
{
    public string Id { get; set; }

    public Format Format { get; set; }
    public Quality Quality { get; set; }
    public string Filename { get; set; }
}