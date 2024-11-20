namespace Toolkit.Foundation;

public interface IAsyncConfirmation
{
    Task<bool> Confirm();
}