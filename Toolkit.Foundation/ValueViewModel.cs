using CommunityToolkit.Mvvm.ComponentModel;

namespace Toolkit.Foundation;

public partial class ValueViewModel<TValue>(IServiceProvider serviceProvider,
    IServiceFactory serviceFactory,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer) :
    ObservableViewModel(serviceProvider, serviceFactory, publisher, subscriber, disposer)
{
    [ObservableProperty]
    private TValue? value;

    protected virtual void OnChanged(TValue? value)
    {

    }

    partial void OnValueChanged(TValue? value) => OnChanged(value);
}
