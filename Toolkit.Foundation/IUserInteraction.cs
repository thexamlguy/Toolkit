namespace Toolkit.Foundation;

public interface IUserInteraction
{
    event EventHandler<UserInteractedEventArgs>? UserInteracted;

    void Stop();

    void Start();
}