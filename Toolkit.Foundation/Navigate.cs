namespace Toolkit.Foundation;

public record Navigate(string Route,
    object? Context = null,
    string? Scope = null,
    object? Sender = null,
    EventHandler? Navigated = null,
    object[]? Parameters = null);

public record Navigate<TNavigation>(object Context,
    object Template,
    object Content,
    object? Sender = null,
    object[]? Parameters = null);