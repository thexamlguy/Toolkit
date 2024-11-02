namespace Toolkit.Foundation;

public record NotifyEventArgs<TSender>(TSender? Sender = default);