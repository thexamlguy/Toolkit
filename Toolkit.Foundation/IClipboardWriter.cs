namespace Toolkit.Foundation;

public interface IClipboardWriter
{
    Task Write<TContent>(TContent content);
}