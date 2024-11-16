using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public partial class Observable(IServiceProvider provider,
    IServiceFactory factory,
    IMessenger messenger,
    IDisposer disposer) :
    ObservableRecipient,
    IObservableViewModel,
    IActivityIndicator,
    IDisposable,
    IServiceProviderRequired,
    IServiceFactoryRequired,
    IMessengerRequired,
    IDisposerRequired
{
    private readonly Dictionary<string, object> trackedProperties = [];

    public IDisposer Disposer { get; } = disposer;

    public IServiceFactory Factory { get; } = factory;

    public IServiceProvider Provider { get; } = provider;

    public IMessenger Messenger { get; } = messenger;

    public void Commit()
    {
        foreach (object trackedProperty in trackedProperties.Values)
        {
            ((dynamic)trackedProperty).Commit();
        }
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
        Disposer.Dispose(this);
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
        IMessenger messenger,
        IDisposer disposer,
        TValue? value = default) : base(provider, factory, messenger, disposer)
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
        IMessenger messenger,
        IDisposer disposer,
        TKey key,
        TValue? value = default) : base(provider, factory, messenger, disposer)
    {
        Key = key;
        Value = value;
    }

    protected virtual void OnValueChanged()
    {
    }

    partial void OnValueChanged(TValue? value) => OnValueChanged();
}