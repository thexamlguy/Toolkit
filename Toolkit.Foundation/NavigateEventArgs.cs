namespace Toolkit.Foundation;

public record NavigateEventArgs(string Route,
    object? Region = null,
    NavigateScope? Scope = null,
    object? Sender = null,
    IDictionary<string, object>? Parameters = null);
