namespace Toolkit.Foundation;

public interface IConfirmation
{
    Task<bool> Confirm();
}