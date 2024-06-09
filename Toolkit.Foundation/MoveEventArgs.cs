namespace Toolkit.Foundation;

public record MoveEventArgs<TSender>(int Index, TSender? Sender = default);