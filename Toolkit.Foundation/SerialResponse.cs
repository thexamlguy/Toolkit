namespace Toolkit.Foundation;

public record SerialResponse<TContent> : 
    ISerialResponse
{
    public SerialResponse(ISerialContext context,
        TContent content)
    {
        Context = context;
        Content = content;
    }

    public ISerialContext Context { get; }

    public TContent Content { get; }
}

public record SerialResponse
{
    public static SerialResponse<TContent> Create<TContent>(ISerialContext context, TContent content)
    {
        return new SerialResponse<TContent>(context, content);
    }
}
