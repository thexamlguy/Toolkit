namespace Toolkit.Foundation;

public interface INavigateBackHandler<TNavigation> :
    INotificationHandler<NavigateBackEventArgs<TNavigation>>,
    INavigateHandler;