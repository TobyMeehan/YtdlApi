namespace YtdlApi.Server;

public class StreamContainer
{
    public StreamContainer(Stream stream, string type, string name)
    {
        Stream = stream;
        Type = type;
        Name = name;
    }

    public Stream Stream { get; }
    public string Type { get; }

    public string Name { get; }
}