namespace Toolkit.Foundation;

public record CreateEventArgs<TSender>(TSender? Sender = default,
    params object[] Parameters);