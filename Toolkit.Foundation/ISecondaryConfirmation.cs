namespace Toolkit.Foundation;

public interface ISecondaryConfirmation
{
    Task<bool> Confirm();
}