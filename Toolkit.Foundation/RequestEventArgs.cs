namespace Toolkit.Foundation;

public record RequestEventArgs<TSender>(TSender? Sender = default);
