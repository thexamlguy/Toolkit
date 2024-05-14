using CommunityToolkit.Mvvm.Input;

namespace Toolkit.Foundation;

public partial class CommandValueViewModel<TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscription subscriber,
    IDisposer disposer) :
    ValueViewModel<TValue>(provider, factory, mediator, publisher, subscriber, disposer)
{
    public IRelayCommand InvokeCommand =>
        new AsyncRelayCommand(InvokeAsync);

    protected virtual Task InvokeAsync() =>
        Task.CompletedTask;
}