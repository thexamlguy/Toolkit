using CommunityToolkit.Mvvm.Input;

namespace Toolkit.Foundation;

public partial class CommandValueViewModel<TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer) :
    Observable<TValue>(provider, factory, mediator, publisher, subscriber, disposer) 
    where TValue : notnull
{
    public IRelayCommand InvokeCommand =>
        new AsyncRelayCommand(InvokeAsync);

    protected virtual Task InvokeAsync() =>
        Task.CompletedTask;
}