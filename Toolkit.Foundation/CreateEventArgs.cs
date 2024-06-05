namespace Toolkit.Foundation;

public record CreateEventArgs<TValue>(TValue Value, params object[] Parameters);