using System.Collections;
using System.Collections.Immutable;
using System.Reactive.Disposables;

namespace Toolkit.Foundation;

public class Cache<TValue>(IDisposer disposer,
    IComparer<TValue>? comparer = default) :
    ICache<TValue>
{
    private readonly List<TValue> cache = [];

    public int IndexOf(TValue value) => 
        cache.IndexOf(value);

    public void Add(TValue value)
    {
        if (value is null)
        {
            return;
        }

        disposer.Add(value, Disposable.Create(() => Remove(value)));
        cache.Add(value);
        cache.Sort(comparer);
    }

    public bool TryGetValue(TValue key, out TValue? value)
    {
        if (cache.FirstOrDefault(x => x is not null && x.Equals(key)) is TValue returningValue)
        {
            value = returningValue;
            return true;
        }

        value = default;
        return false;
    }

    public void Clear() => cache.Clear();

    public IEnumerator<TValue> GetEnumerator() => cache.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Remove(TValue value) => cache.Remove(value);
}

public class Cache<TKey, TValue>(IComparer<TKey> comparer) :
    ICache<TKey, TValue>
    where TKey :
    notnull
    where TValue :
    notnull
{
    private readonly SortedList<TKey, TValue> cache = new(comparer);

    public void Add(TKey key,
        TValue value)
    {
        cache.TryAdd(key, value);
    }

    public void Clear() => cache.Clear();

    public bool ContainsKey(TKey key) => cache.ContainsKey(key);

    public int IndexOf(TKey key) => cache.IndexOfKey(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => cache.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Remove(TKey key) => cache.Remove(key, out _);

    public bool TryGetValue(TKey key, out TValue? value)
    {
        if (cache.TryGetValue(key, out value))
        {
            return true;
        }

        value = default;
        return false;
    }
}