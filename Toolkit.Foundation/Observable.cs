using CommunityToolkit.Mvvm.ComponentModel;

namespace Toolkit.Foundation;

public partial class Observable(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer) :
    ObservableObject,
    IObservableViewModel,
    IActivityIndicator,
    IInitialization,
    IActivated,
    IDeactivating,
    IDeactivated,
    IDisposable,
    IServiceProviderRequired,
    IServiceFactoryRequired,
    IMediatorRequired,
    IPublisherRequired,
    IDisposerRequired
{
    private readonly Dictionary<string, object> trackedProperties = [];

    [ObservableProperty]
    private bool isActivated;

    [ObservableProperty]
    private bool isActive;

    [ObservableProperty]
    private bool isInitialized;

    public IDisposer Disposer { get; } = disposer;

    public IServiceFactory Factory { get; } = factory;

    public IMediator Mediator { get; } = mediator;

    public IServiceProvider Provider { get; } = provider;

    public IPublisher Publisher { get; } = publisher;

    public ISubscriber Subscriber { get; } = subscriber;

    public virtual Task OnActivated()
    {
        IsActivated = true;
        return Task.CompletedTask;
    }

    public void Commit()
    {
        foreach (object trackedProperty in trackedProperties.Values)
        {
            ((dynamic)trackedProperty).Commit();
        }
    }

    public virtual Task OnDeactivated()
    {
        IsActivated = false;
        return Task.CompletedTask;
    }

    public virtual Task OnDeactivating() =>
        Task.CompletedTask;

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        Disposer.Dispose(this);
    }

    public virtual void OnInitialize()
    {
    }

    public virtual void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        IsInitialized = true;
        Subscriber.Subscribe(this);
        OnInitialize();
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
{
    [ObservableProperty]
    private TValue? value;

    public Observable(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer,
        TValue? value = default) : base(provider, factory, mediator, publisher, subscriber, disposer)
    {
        Value = value;
    }

    protected virtual void OnChanged(TValue? value)
    {
    }

    partial void OnValueChanged(TValue? value) => OnChanged(value);
}

public partial class Observable<TKey, TValue> :
    Observable
{
    [ObservableProperty]
    private TKey key;

    [ObservableProperty]
    private TValue? value;

    public Observable(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer,
        TKey key,
        TValue? value = default) : base(provider, factory, mediator, publisher, subscriber, disposer)
    {
        Key = key;
        Value = value;
    }

    protected virtual void OnValueChanged()
    {
    }

    partial void OnValueChanged(TValue? value) => OnValueChanged();
}