using CommunityToolkit.Mvvm.Input;

namespace Toolkit.Foundation;

public partial class CommandValueViewModel<TValue>(IServiceProvider serviceProvider,
    IServiceFactory serviceFactory,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer) :
    ValueViewModel<TValue>(serviceProvider, serviceFactory, publisher, subscriber, disposer)
{
    public IRelayCommand InvokeCommand =>
        new AsyncRelayCommand(InvokeAsync);

    protected virtual Task InvokeAsync() =>
        Task.CompletedTask;
}