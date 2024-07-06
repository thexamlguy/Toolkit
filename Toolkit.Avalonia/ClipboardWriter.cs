using Avalonia.Controls;
using Avalonia.Input.Platform;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ClipboardWriter(ITopLevelProvider topLevelProvider) :
    IClipboardWriter
{
    public async Task Write<TContent>(TContent content)
    {
        if (topLevelProvider.Get() is TopLevel topLevel)
        {
            if (topLevel.Clipboard is IClipboard clipboard)
            {
                if (content is string stringContent)
                {
                    await clipboard.SetTextAsync(stringContent);
                }
            }
        }
    }
}