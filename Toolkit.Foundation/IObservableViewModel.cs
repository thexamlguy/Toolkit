using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public interface IObservableViewModel :
    IDisposable
{
    public IDisposer Disposer { get; }

    public IMessenger Messenger { get; }

    public IServiceFactory Factory { get; }

    public IServiceProvider Provider { get; }
}