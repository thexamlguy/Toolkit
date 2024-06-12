using CommunityToolkit.Mvvm.ComponentModel;

namespace Toolkit.Foundation;

public partial class ValueViewModel<TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer) :
    Observable(provider, factory, mediator, publisher, subscriber, disposer)
{
    [ObservableProperty]
    private TValue? value;

    protected virtual void OnChanged(TValue? value)
    {
    }

    partial void OnValueChanged(TValue? value) => OnChanged(value);
}