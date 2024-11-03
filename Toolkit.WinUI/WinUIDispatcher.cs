using Microsoft.UI.Dispatching;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

public class WinUIDispatcher :
    IDispatcher
{
    public Task Invoke(Action action)
    {
        DispatcherQueue.GetForCurrentThread().TryEnqueue(action.Invoke);
        return Task.CompletedTask;
    }
}
