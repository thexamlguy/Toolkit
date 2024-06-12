using CommunityToolkit.Mvvm.ComponentModel;

namespace Toolkit.Foundation;

public partial class Observable(IServiceProvider
        provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer) :
    ObservableObject,
    IObservableViewModel,
    IActivityIndicator,
    IPostInitialization,
    IInitialization,
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
    private readonly Dictionary<string, object> trackedProperties = [];

    [ObservableProperty]
    private bool active;

    [ObservableProperty]
    private bool initialized;
    private bool postInitialized;

    public event EventHandler? DeactivateHandler;

    public IDisposer Disposer { get; } = disposer;

    public IServiceFactory Factory { get; } = factory;

    public IMediator Mediator { get; } = mediator;

    public IServiceProvider Provider { get; } = provider;

    public IPublisher Publisher { get; } = publisher;
    public ISubscriber Subscriber { get; } = subscriber;

    public void Commit()
    {
        foreach (object trackedProperty in trackedProperties.Values)
        {
            ((dynamic)trackedProperty).Commit();
        }
    }

    public Task Deactivate()
    {
        DeactivateHandler?.Invoke(this, new EventArgs());
        return Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        Disposer.Dispose(this);
    }

    public Task Initialize()
    {
        if (Initialized)
        {
            return Task.CompletedTask;
        }

        Initialized = true;
        return Task.CompletedTask;
    }

    public virtual Task OnActivated() =>
        Task.CompletedTask;

    public virtual Task OnDeactivated() =>
        Task.CompletedTask;

    public virtual Task OnDeactivating() =>
        Task.CompletedTask;

    public virtual void PostInitialize()
    {
        if (postInitialized)
        {
            return;
        }

        postInitialized = true;
        Subscriber.Subscribe(this);
    }

    public void Revert()
    {
        foreach (object trackedProperty in trackedProperties.Values)
        {
            ((dynamic)trackedProperty).Revert();
        }
    }

    public void Track<T>(string propertyName, Func<T> getter, Action<T> setter)
    {
        if (!trackedProperties.ContainsKey(propertyName))
        {
            T initialValue = getter();
            trackedProperties[propertyName] = new TrackedProperty<T>(initialValue, setter, getter);
        }
    }
}

public partial class Observable<TValue> :
    Observable
    where TValue : notnull
{
    [ObservableProperty]
    private TValue value;

    public Observable(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer,
        TValue value) : base(provider, factory, mediator, publisher, subscriber, disposer)
    {
        Value = value;
    }

    protected virtual void OnValueChanged()
    {

    }

    partial void OnValueChanged(TValue value) => OnValueChanged();
}

public partial class Observable<TKey, TValue> :
    Observable
    where TKey : notnull
    where TValue : notnull
{
    [ObservableProperty]
    private TKey key;

    [ObservableProperty]
    private TValue value;

    public Observable(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher, 
        ISubscriber subscriber, 
        IDisposer disposer,
        TKey key,
        TValue value) : base(provider, factory, mediator, publisher, subscriber, disposer)
    {
        Key = key;
        Value = value;
    }

    protected virtual void OnValueChanged()
    {

    }

    partial void OnValueChanged(TValue value) => OnValueChanged();
}