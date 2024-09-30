namespace Toolkit.Foundation;

public record PersistEventArgs<TSender>(TSender? Sender = default);