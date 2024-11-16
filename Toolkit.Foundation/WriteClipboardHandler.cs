namespace Toolkit.Foundation;

public class WriteClipboardHandler(IClipboardWriter clipboardWriter) :
    IAsyncHandler<WriteEventArgs<Clipboard<object>>>
{
    public async Task Handle(WriteEventArgs<Clipboard<object>> args,
        CancellationToken cancellationToken = default)
    {
        if (args.Sender is Clipboard<object> clipboard)
        {
            await clipboardWriter.Write(clipboard.Value);
        }
    }
}