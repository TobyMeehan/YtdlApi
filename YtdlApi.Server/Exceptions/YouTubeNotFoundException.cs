namespace YtdlApi.Server.Exceptions;

public class YouTubeNotFoundException : Exception
{
    public YouTubeNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}