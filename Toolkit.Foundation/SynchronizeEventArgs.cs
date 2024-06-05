namespace Toolkit.Foundation;

public record SynchronizeEventArgs<TSynchronize, TValue>(TValue? Value = default) :
    ISynchronize;

public record SynchronizeEventArgs<TSynchronize>() :
    ISynchronize;