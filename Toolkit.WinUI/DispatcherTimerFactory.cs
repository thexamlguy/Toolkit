using Toolkit.Foundation;

namespace Toolkit.WinUI;

public class DispatcherTimerFactory : 
    IDispatcherTimerFactory
{
    public IDispatcherTimer Create(Action actionDelegate, TimeSpan interval) => 
        new DispatcherTimer(actionDelegate, interval);
}