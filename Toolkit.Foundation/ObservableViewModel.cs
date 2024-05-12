using CommunityToolkit.Mvvm.ComponentModel;

namespace Toolkit.Foundation;

public partial class ObservableViewModel :
    ObservableObject,
    IObservableViewModel,
    IInitializer,
    IActivated,
    IDeactivating,
    IDeactivated,
    IDeactivatable
{
    [ObservableProperty]
    private bool isInitialized;

    public ObservableViewModel(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
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

    public virtual Task Activated() =>
        Task.CompletedTask;

    public Task Deactivate()
    {
        DeactivateHandler?.Invoke(this, new EventArgs());
        return Task.CompletedTask;
    }

    public virtual Task Deactivated() =>
        Task.CompletedTask;

    public virtual Task Deactivating() =>
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

public partial class ObservableViewModel<TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator, 
    IPublisher publisher, 
    ISubscriber subscriber, 
    IDisposer disposer) : ObservableViewModel(provider, factory, mediator, publisher, subscriber, disposer)
    where TValue : notnull
{
    [ObservableProperty]
    private TValue? value;
}

public partial class ObservableViewModel<TKey, TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer,
    TValue? value = null) : ObservableViewModel(provider, factory, mediator, publisher, subscriber, disposer)
    where TValue : class
{
    [ObservableProperty]
    private TKey? key;

    [ObservableProperty]
    private TValue? value = value;
}