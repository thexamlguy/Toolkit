using CommunityToolkit.Mvvm.Messaging;
using Toolkit.Foundation;

namespace Toolkit.Windows;

public class HotKeyListener(ICache<int, HotKeyDescriptor> cache,
    IMessenger messenger) : 
    IHotKeyListener,
    IRecipient<WndProcEventArgs>
{
    private bool isDisposed;

    ~HotKeyListener()
    {
        Dispose(false);
    }

    public void Initialize()
    {
        messenger.RegisterAll(this);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            isDisposed = true;
        }
    }

    public void Receive(WndProcEventArgs message)
    {
        const int WM_HOTKEY = 0x0312;
        if (message.Message == WM_HOTKEY)
        {
            int key = (int)message.WParam;
            if (cache.Contains(key))
            {
                messenger.Send(new HotKeyPressedEventArgs(key));
            }
        }
    }
}
