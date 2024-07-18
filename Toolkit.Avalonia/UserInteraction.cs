using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;


public class UserInteraction(ITopLevelProvider topLevelProvider) : 
    IUserInteraction
{
    public event EventHandler<UserInteractedEventArgs>? UserInteracted;

    private void OnPointerMoved(object? sender,
        PointerEventArgs args) => UserInteracted?.Invoke(this, new UserInteractedEventArgs());

    private void OnKeyDown(object? sender,
        KeyEventArgs args) => UserInteracted?.Invoke(this, new UserInteractedEventArgs());

    private void OnKeyUp(object? sender,
        KeyEventArgs args) => UserInteracted?.Invoke(this, new UserInteractedEventArgs());

    public void Stop()
    {
        if (topLevelProvider.Get() is TopLevel topLevel)
        {
            topLevel.RemoveHandler(InputElement.PointerMovedEvent, OnPointerMoved);
            topLevel.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);
            topLevel.RemoveHandler(InputElement.KeyUpEvent, OnKeyUp);
        }
    }

    public void Start()
    {
        if (topLevelProvider.Get() is TopLevel topLevel)
        {
            topLevel.AddHandler(InputElement.PointerMovedEvent, OnPointerMoved, RoutingStrategies.Tunnel);
            topLevel.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
            topLevel.AddHandler(InputElement.KeyUpEvent, OnKeyUp, RoutingStrategies.Tunnel);
        }
    }
}
