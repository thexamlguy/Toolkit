namespace Toolkit.Foundation;

public interface IDispatcherTimerFactory
{
    IDispatcherTimer Create(Action actionDelegate, TimeSpan interval);
}