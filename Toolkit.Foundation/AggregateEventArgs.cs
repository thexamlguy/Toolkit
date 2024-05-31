namespace Toolkit.Foundation;

public record AggregateEventArgs<TValue, TOptions>(TOptions? Options = null) :
    IAggregate
    where TOptions : class;

public record AggerateEventArgs<TValue> :
    IAggregate;