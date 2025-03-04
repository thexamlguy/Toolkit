namespace Toolkit.Windows;

public interface IHotKeyManager :
    IDisposable
{
    void Add(int key, HotKeyDescriptor descriptor);

    bool Contains(int key);

    void Remove(int key);
}
