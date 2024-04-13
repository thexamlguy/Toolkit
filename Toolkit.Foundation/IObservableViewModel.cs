namespace Toolkit.Foundation;

public interface IObservableViewModel :
    IDisposable
{
    public IDisposer Disposer { get; }

    public IPublisher Publisher { get; }

    public IServiceFactory ServiceFactory { get; }

    public IServiceProvider ServiceProvider { get; }
}