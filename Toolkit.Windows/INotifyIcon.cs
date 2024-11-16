using Toolkit.Foundation;

namespace Toolkit.Windows;

public interface INotifyIcon :
    IInitialization,
    IDisposable
{
    void SetIcon(IntPtr iconHandle);
}
