using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public partial class CommandValueViewModel<TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMessenger messenger,
    IDisposer disposer) :
    Observable<TValue>(provider, factory, messenger, disposer) 
    where TValue : notnull
{
    public IRelayCommand InvokeCommand =>
        new AsyncRelayCommand(InvokeAsync);

    protected virtual Task InvokeAsync() =>
        Task.CompletedTask;
}