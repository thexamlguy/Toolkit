namespace Toolkit.Foundation;

public interface IDeactivating
{
    Task Deactivating();
}

public interface IDeactivating<TResult> 
{
    Task<TResult> Deactivating();
}