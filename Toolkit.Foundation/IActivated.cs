namespace Toolkit.Foundation;

public interface IActivated
{
    Task OnActivated();
}

public interface IActivated<TResult>
{
    Task Activated(TResult result);
}
