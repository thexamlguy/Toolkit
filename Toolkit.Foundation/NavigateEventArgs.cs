namespace Toolkit.Foundation;

public record NavigateEventArgs(string Route,
    object? Context = null,
    string? Scope = null,
    object? Sender = null,
    EventHandler? Navigated = null,
    object[]? Parameters = null);

public record NavigateEventArgs<TNavigation>(object Context,
    object Template,
    object Content,
    object? Sender = null,
    object[]? Parameters = null);