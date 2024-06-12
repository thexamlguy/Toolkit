using CommunityToolkit.Mvvm.Input;

namespace Toolkit.Foundation;

public partial class CommandViewModel(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer) :
    Observable(provider, factory, mediator, publisher, subscriber, disposer)
{
    public IRelayCommand InvokeCommand =>
        new AsyncRelayCommand(InvokeAsync);

    protected virtual Task InvokeAsync() =>
        Task.CompletedTask;
}