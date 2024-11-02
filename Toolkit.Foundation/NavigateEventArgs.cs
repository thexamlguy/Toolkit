namespace Toolkit.Foundation;

public record NavigateEventArgs(string Route,
    object? Region = null,
    string? Scope = null,
    object? Sender = null,
    EventHandler? Navigated = null,
    IDictionary<string, object>? Parameters = null);

public record NavigateEventArgs<TNavigation>(object Region,
    object Template,
    object Content,
    object? Sender = null,
    IDictionary<string, object>? Parameters = null);