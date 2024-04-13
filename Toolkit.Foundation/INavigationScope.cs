namespace Toolkit.Foundation;

public interface INavigationScope
{
    Task NavigateAsync(string route, object? sender = null, object? context = null, 
        EventHandler? navigated = null, object[]? parameters = null, CancellationToken cancellationToken = default);

    Task NavigateBackAsync(object? context, CancellationToken cancellationToken = default);
}

