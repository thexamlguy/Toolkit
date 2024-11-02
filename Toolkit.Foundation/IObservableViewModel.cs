namespace Toolkit.Foundation;

public interface IObservableViewModel :
    IDisposable
{
    public IDisposer Disposer { get; }

    public IPublisher Publisher { get; }

    public IServiceFactory Factory { get; }

    public IServiceProvider Provider { get; }
}