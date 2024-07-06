namespace Toolkit.Foundation;

public class WriteClipboardHandler(IClipboardWriter clipboardWriter) :
    INotificationHandler<WriteEventArgs<Clipboard<object>>>
{
    public async Task Handle(WriteEventArgs<Clipboard<object>> args)
    {
        if (args.Sender is Clipboard<object> clipboard)
        {
            await clipboardWriter.Write(clipboard.Value);
        }
    }
}
