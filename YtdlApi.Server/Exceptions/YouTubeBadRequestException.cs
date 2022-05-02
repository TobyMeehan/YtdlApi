namespace YtdlApi.Server.Exceptions;

public class YouTubeBadRequestException : Exception
{
    public YouTubeBadRequestException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}