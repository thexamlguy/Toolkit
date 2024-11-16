using Toolkit.Foundation;

namespace Toolkit.Windows;

public interface ITaskbar :
    IInitialization, 
    IDisposable
{
    TaskbarState GetCurrentState();

    IntPtr GetHandle();
}