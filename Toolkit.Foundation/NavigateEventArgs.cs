namespace Toolkit.Foundation;

public record NavigateEventArgs(string Route,
    object? Region = null,
    string? Scope = null,
    object? Sender = null,
    EventHandler? Navigated = null,
    object[]? Parameters = null);

public record NavigateEventArgs<TNavigation>(object Region,
    object Template,
    object Content,
    object? Sender = null,
    object[]? Parameters = null);