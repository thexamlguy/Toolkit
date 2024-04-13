namespace Toolkit.Foundation;

public record NavigatingFrom(object Content) :
    IRequest<IReadOnlyCollection<object>>;

public record NavigatingTo(object Content) :
    IRequest<IReadOnlyCollection<object>>;
