using Microsoft.UI.Dispatching;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

public class WinUIDispatcher(DispatcherQueue dispatcherQueue) :
    IDispatcher
{
    public bool CheckAccess() => dispatcherQueue.HasThreadAccess;

    public Task Invoke(Action action)
    {
        dispatcherQueue.TryEnqueue(action.Invoke);
        return Task.CompletedTask;
    }
}
