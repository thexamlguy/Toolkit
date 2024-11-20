namespace Toolkit.Foundation;

public interface IAsyncSecondaryConfirmation
{
    Task<bool> ConfirmSecondary();
}