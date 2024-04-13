using System.Reactive.Disposables;
using System.Collections;
using System.Collections.Concurrent;

namespace Toolkit.Foundation;

public class Disposer : 
    IDisposer
{
    private readonly ConcurrentDictionary<object, CompositeDisposable> subjects = [];

    public void Add(object subject,
        params object[] objects)
    {
        CompositeDisposable disposables = subjects.GetOrAdd(subject, args => new CompositeDisposable());
        foreach (IDisposable disposable in objects.OfType<IDisposable>())
        {
            disposables.Add(disposable);
        }

        foreach (object notDisposable in objects.Where(x => x is not IDisposable))
        {
            disposables.Add(Disposable.Create(() => FromNotDisposable(notDisposable)));
        }
    }

    private void FromNotDisposable(object target)
    {
        if (target is IList collection && collection is { Count: > 0 })
        {
            foreach (object? item in collection)
            {
                FromNotDisposable(item);
            }
        }

        if (target is IDisposable disposable)
        {
            disposable.Dispose();
        }

        if (target is not IDisposable)
        {
            Dispose(target);
        }
    }

    public TDisposable Replace<TDisposable>(object subject, 
        IDisposable disposer, 
        TDisposable replacement)
        where TDisposable : 
        IDisposable
    {
        CompositeDisposable disposables = subjects.GetOrAdd(subject, args => new CompositeDisposable());
        if (disposer is not null)
        {
            disposables.Remove(disposer);
        }

        disposables.Add(replacement);
        return replacement;
    }

    public void Remove(object subject, 
        IDisposable disposer)
    {
        CompositeDisposable disposables = subjects.GetOrAdd(subject, args => new CompositeDisposable());
        if (disposer is not null)
        {
            disposables.Remove(disposer);
        }
    }

    public void Dispose(object subject)
    {
        if (subjects.TryGetValue(subject, out CompositeDisposable? disposables))
        {
            disposables?.Dispose();
        }
    }
}
