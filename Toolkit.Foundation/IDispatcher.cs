namespace Toolkit.Foundation;

public interface IDispatcher
{
    Task Invoke(Action action);
}