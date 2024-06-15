namespace Toolkit.Foundation;

public interface INavigation
{
    void Navigate(string route,
        object? sender = null,
        object? region = null,
        EventHandler? navigated = null,
        IDictionary<string, object>? parameters = null);

    void Back(object? region);
}