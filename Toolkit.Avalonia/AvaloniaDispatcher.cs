using Avalonia.Threading;
using IDispatcher = Toolkit.Foundation.IDispatcher;

namespace Toolkit.Avalonia;

public class AvaloniaDispatcher :
    IDispatcher
{
    public bool CheckAccess() =>
        Dispatcher.UIThread.CheckAccess();

    public async Task Invoke(Action action) => 
        await Dispatcher.UIThread.InvokeAsync(action);
}