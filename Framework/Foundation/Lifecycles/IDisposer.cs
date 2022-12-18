namespace Toolkit.Framework.Foundation;

public interface IDisposer
{
    void Add(object subject, params object[] objects);

    void Remove(object subject, IDisposable disposer);

    void Dispose(object subject);
}
