namespace Toolkit.Foundation;

public delegate Task NotificationHandlerDelegate<TNotification>(TNotification notification,
    CancellationToken cancellationToken)
    where TNotification : INotification;
