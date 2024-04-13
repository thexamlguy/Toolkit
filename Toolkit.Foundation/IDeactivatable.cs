namespace Toolkit.Foundation;

public interface IDeactivatable
{
    public event EventHandler? DeactivateHandler;

    public Task Deactivate();
}