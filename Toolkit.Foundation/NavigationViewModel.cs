using CommunityToolkit.Mvvm.ComponentModel;

namespace Toolkit.Foundation;

public partial class NavigationViewModel(IServiceProvider serviceProvider,
    IServiceFactory serviceFactory,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer,
    string text) :
    ObservableViewModel(serviceProvider, serviceFactory, publisher, subscriber, disposer),
    INavigationViewModel
{
    [ObservableProperty]
    private string? text = text;
}

public partial class NavigationViewModel<TNavigationViewModel>(IServiceProvider serviceProvider,
    IServiceFactory serviceFactory,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer,
    string text) :
    ObservableViewModel(serviceProvider, serviceFactory, publisher, subscriber, disposer),
    INavigationViewModel
    where TNavigationViewModel :
    INavigationViewModel
{
    [ObservableProperty]
    private string? text = text;
}
