using CommunityToolkit.Mvvm.ComponentModel;

namespace Toolkit.Foundation;

public partial class Observable :
    ObservableObject,
    IObservableViewModel,
    IInitializer,
    IActivated,
    IDeactivating,
    IDeactivated,
    IDeactivatable,
    IDisposable,
    IServiceProviderRequired,
    IServiceFactoryRequired,
    IMediatorRequired,
    IPublisherRequired,
    IDisposerRequired
{
    [ObservableProperty]
    private bool isInitialized;

    public Observable(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscription subscriber,
        IDisposer disposer)
    {
        Provider = provider;
        Factory = factory;
        Mediator = mediator;
        Publisher = publisher;
        Disposer = disposer;

        subscriber.Add(this);
    }

    public event EventHandler? DeactivateHandler;

    public IDisposer Disposer { get; }

    public IServiceFactory Factory { get; }

    public IMediator Mediator { get; }

    public IServiceProvider Provider { get; }

    public IPublisher Publisher { get; }

    public virtual Task OnActivated() =>
        Task.CompletedTask;

    public Task Deactivate()
    {
        DeactivateHandler?.Invoke(this, new EventArgs());
        return Task.CompletedTask;
    }

    public virtual Task OnDeactivated() =>
        Task.CompletedTask;

    public virtual Task OnDeactivating() =>
        Task.CompletedTask;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Disposer.Dispose(this);
    }

    public Task Initialize()
    {
        if (IsInitialized)
        {
            return Task.CompletedTask;
        }

        IsInitialized = true;
        return Task.CompletedTask;
    }
}

public partial class Observable<TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator, 
    IPublisher publisher, 
    ISubscription subscriber, 
    IDisposer disposer,
    TValue? value = default) : Observable(provider, factory, mediator, publisher, subscriber, disposer)
{
    [ObservableProperty]
    private TValue? value = value;
}


public partial class Observable<TKey, TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscription subscriber,
    IDisposer disposer,
    TValue? value = default) : Observable(provider, factory, mediator, publisher, subscriber, disposer)
{
    [ObservableProperty]
    private TKey? key;

    [ObservableProperty]
    private TValue? value = value;
}