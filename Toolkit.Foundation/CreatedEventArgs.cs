namespace Toolkit.Foundation;

public record CreatedEventArgs<TSender>(TSender? Sender = default);