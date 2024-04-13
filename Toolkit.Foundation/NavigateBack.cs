namespace Toolkit.Foundation;

public record NavigateBack(object? Context = null, string? Scope = null) :
    INotification;

public record NavigateBack<TNavigation>(object? Context) :
    INotification;
