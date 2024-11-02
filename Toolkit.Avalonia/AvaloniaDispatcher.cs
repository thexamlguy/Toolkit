using Avalonia.Threading;
using IDispatcher = Toolkit.Foundation.IDispatcher;

namespace Toolkit.Avalonia;

public class AvaloniaDispatcher :
    IDispatcher
{
    public async Task Invoke(Action action) => 
        await Dispatcher.UIThread.InvokeAsync(action);
}