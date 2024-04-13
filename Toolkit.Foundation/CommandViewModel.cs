using CommunityToolkit.Mvvm.Input;

namespace Toolkit.Foundation;

public partial class CommandViewModel(IServiceProvider serviceProvider,
    IServiceFactory serviceFactory,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer) :
    ObservableViewModel(serviceProvider, serviceFactory, publisher, subscriber, disposer)
{
    public IRelayCommand InvokeCommand => 
        new AsyncRelayCommand(InvokeAsync);

    protected virtual Task InvokeAsync() => 
        Task.CompletedTask;
}