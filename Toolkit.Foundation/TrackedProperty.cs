namespace Toolkit.Foundation;

public class TrackedProperty<T>(T initial,
    Action<T> revert,
    Func<T> commit)
{
    public void Commit() => initial = commit();

    public void Revert() => revert(initial);
}
