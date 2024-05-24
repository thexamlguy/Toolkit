namespace Toolkit.Foundation;

public record ChangedEventArgs<TValue>(TValue? Value = default);