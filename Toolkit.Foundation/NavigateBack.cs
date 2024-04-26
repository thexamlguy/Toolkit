namespace Toolkit.Foundation;

public record NavigateBack(object? Context = null, string? Scope = null);

public record NavigateBack<TNavigation>(object? Context);