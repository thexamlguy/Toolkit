namespace Toolkit.Foundation;

public record ReplaceEventArgs<TSender>(int Index, TSender? Sender = default);