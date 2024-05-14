namespace Toolkit.Foundation;

public interface INavigationScope
{
    void Navigate(string route, object? sender = null, object? context = null,
        EventHandler? navigated = null, object[]? parameters = null);

    void Back(object? context);
}