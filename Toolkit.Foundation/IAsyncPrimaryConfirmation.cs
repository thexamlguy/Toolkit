namespace Toolkit.Foundation;

public interface IAsyncPrimaryConfirmation
{
    Task<bool> ConfirmPrimary();
}