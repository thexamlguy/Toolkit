namespace Toolkit.Foundation;

public record AggerateEventArgs<TType, TValue>(TValue? Value = default) :
    IAggregate;

public record AggerateEventArgs<TType> :
    IAggregate;