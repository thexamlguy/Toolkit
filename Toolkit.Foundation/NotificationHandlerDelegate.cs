namespace Toolkit.Foundation;

public delegate Task NotificationHandlerDelegate<TMessage>(TMessage message,
    CancellationToken cancellationToken);
