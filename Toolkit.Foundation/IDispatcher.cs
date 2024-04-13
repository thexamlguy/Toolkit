namespace Toolkit.Foundation;

public interface IDispatcher
{
    Task InvokeAsync(Action action);
}
