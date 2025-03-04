using Toolkit.Foundation;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace Toolkit.Windows;

public class HotKeyManager(IWndProc wndProc, 
    ICache<int, HotKeyDescriptor> cache) :
    IHotKeyManager
{
    public void Add(int key, HotKeyDescriptor descriptor)
    {
        HOT_KEY_MODIFIERS modifiers = 0;

        if ((descriptor.Modifiers & ModifierKey.Alt) == ModifierKey.Alt)
            modifiers |= HOT_KEY_MODIFIERS.MOD_ALT;
        if ((descriptor.Modifiers & ModifierKey.Ctrl) == ModifierKey.Ctrl)
            modifiers |= HOT_KEY_MODIFIERS.MOD_CONTROL;
        if ((descriptor.Modifiers & ModifierKey.Shift) == ModifierKey.Shift)
            modifiers |= HOT_KEY_MODIFIERS.MOD_SHIFT;
        if ((descriptor.Modifiers & ModifierKey.Win) == ModifierKey.Win)
            modifiers |= HOT_KEY_MODIFIERS.MOD_WIN;

        uint vk = (uint)descriptor.VirtualKey;
        if (PInvoke.RegisterHotKey(new HWND(wndProc.Handle), key, modifiers, vk))
        {
            cache.Add(key, descriptor);
        }
    }

    public bool Contains(int key) =>
        cache.Contains(key);

    public void Remove(int key)
    {
        PInvoke.UnregisterHotKey(new HWND(wndProc.Handle), key);
        cache.Remove(key);
    }

    public void Dispose()
    {
        HWND hwnd = new(wndProc.Handle);
        foreach (KeyValuePair<int, HotKeyDescriptor> item in cache)
        {
            PInvoke.UnregisterHotKey(hwnd, item.Key);
        }

        cache.Clear();
        GC.SuppressFinalize(this);
    }

    ~HotKeyManager()
    {
        Dispose();
    }
}
