namespace Toolkit.Foundation;

public interface IMicroControllerContext<TRead, TEvent> :
    IMicroControllerContext 
    where TEvent : ISerialEventArgs<TRead>;

public interface IMicroControllerContext;