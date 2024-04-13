using CommunityToolkit.Mvvm.ComponentModel;

namespace Toolkit.Foundation;

public  partial class ObservableViewModel : 
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

    public ObservableViewModel(IServiceProvider serviceProvider,
        IServiceFactory serviceFactory,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer)
    {
        ServiceProvider = serviceProvider;
        ServiceFactory = serviceFactory;
        Publisher = publisher;
        Disposer = disposer;

        subscriber.Add(this);
    }

    public event EventHandler? DeactivateHandler;

    public IDisposer Disposer { get; }

    public IPublisher Publisher { get; }

    public IServiceFactory ServiceFactory { get; }

    public IServiceProvider ServiceProvider { get; }

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
