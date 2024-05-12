namespace Toolkit.Foundation;

public interface IDeactivating
{
    Task OnDeactivating();
}

public interface IDeactivating<TResult>
{
    Task<TResult> Deactivating();
}