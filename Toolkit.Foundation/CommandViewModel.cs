using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public partial class CommandViewModel(IServiceProvider provider,
    IServiceFactory factory,
    IMessenger messenger,
    IDisposer disposer) :
    Observable(provider, factory, messenger, disposer)
{
    public IRelayCommand InvokeCommand =>
        new AsyncRelayCommand(InvokeAsync);

    protected virtual Task InvokeAsync() =>
        Task.CompletedTask;
}