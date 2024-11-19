namespace Toolkit.Foundation;

public record NavigateEventArgs(string Route,
    object? Region = null,
    string? Scope = null,
    object? Sender = null,
    EventHandler? Navigated = null,
    IDictionary<string, object>? Parameters = null);
