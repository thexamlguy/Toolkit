namespace Toolkit.Foundation;

public interface INavigateHandler;

public interface INavigateHandler<TNavigation> :
    INotificationHandler<NavigateEventArgs<TNavigation>>,
    INavigateHandler;