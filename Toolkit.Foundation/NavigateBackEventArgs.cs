namespace Toolkit.Foundation;

public record NavigateBackEventArgs(object? Context = null, string? Scope = null);

public record NavigateBackEventArgs<TNavigation>(object? Context);