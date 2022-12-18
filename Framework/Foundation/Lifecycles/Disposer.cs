using System.Runtime.CompilerServices;
using System.Reactive.Disposables;

namespace Toolkit.Framework.Foundation;

public class Disposer : IDisposer
{
    private readonly ConditionalWeakTable<object, CompositeDisposable> subjects = new();

    public void Add(object subject, params object[] objects)
    {
        CompositeDisposable disposables = subjects.GetOrCreateValue(subject);
        foreach (IDisposable disposable in objects.OfType<IDisposable>())
        {
            disposables.Add(disposable);
        }
    }

    public TDisposable Replace<TDisposable>(object subject, IDisposable disposer, TDisposable replacement) where TDisposable : IDisposable
    {
        CompositeDisposable disposables = subjects.GetOrCreateValue(subject);
        if (disposer is not null)
        {
            disposables.Remove(disposer);
        }

        disposables.Add(replacement);
        return replacement;
    }
    public void Remove(object subject, IDisposable disposer)
    {
        CompositeDisposable disposables = subjects.GetOrCreateValue(subject);
        if (disposer is not null)
        {
            disposables.Remove(disposer);
        }
    }

    public void Dispose(object subject)
    {
        if (subjects.TryGetValue(subject, out CompositeDisposable disposables))
        {
            disposables.Dispose();
        }
    }
}
