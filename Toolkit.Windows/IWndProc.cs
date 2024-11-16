using Toolkit.Foundation;

namespace Toolkit.Windows;

public interface IWndProc :
    IInitialization, 
    IDisposable
{

    IntPtr Handle { get; }
}
