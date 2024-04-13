namespace Toolkit.Foundation;

public interface INavigateBackHandler<TNavigation> :
    INotificationHandler<NavigateBack<TNavigation>>,
    INavigateHandler;