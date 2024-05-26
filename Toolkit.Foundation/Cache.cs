using System.Collections;

namespace Toolkit.Foundation;

public class Cache<TValue>(IComparer<TValue>? comparer = default) :
    ICache<TValue>
{
    private readonly List<TValue> items = [];

    public IEnumerable<TValue> Items =>
        items;

    public TValue this[int index] =>
        items[index];

    public void Add(TValue item)
    {
        int index = items.BinarySearch(item, comparer);
        if (index < 0)
        {
            index = ~index;
        }
        items.Insert(index, item);
    }

    public void Clear() => items.Clear();

    public IEnumerator<TValue> GetEnumerator() =>
        items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

    public int IndexOf(TValue item) =>
        items.BinarySearch(item, comparer);

    public bool Remove(TValue item)
    {
        int index = items.BinarySearch(item, comparer);
        if (index >= 0)
        {
            items.RemoveAt(index);
            return true;
        }
        return false;
    }

    public bool Contains(TValue key)
    {
        int index = items.BinarySearch(key, comparer);
        if (index >= 0)
        {
            return true;
        }

        return false;
    }

    public bool TryGetValue(TValue key, out TValue? item)
    {
        int index = items.BinarySearch(key, comparer);
        if (index >= 0)
        {
            item = items[index];
            return true;
        }

        item = default;
        return false;
    }
}

public class Cache<TKey, TValue>(IComparer<TKey> comparer) :
    ICache<TKey, TValue>
    where TKey :
    notnull
    where TValue :
    notnull
{
    private readonly List<KeyValuePair<TKey, TValue?>> items = [];

    public TValue? this[TKey key]
    {
        get
        {
            int index = items.BinarySearch(new KeyValuePair<TKey, TValue?>(key, default),
                new KeyValuePairComparer<TKey, TValue?>(comparer));

            if (index >= 0)
            {
                return items[index].Value;
            }

            return default;
        }
        set
        {
            int index = items.BinarySearch(new KeyValuePair<TKey, TValue?>(key, default),
                new KeyValuePairComparer<TKey, TValue?>(comparer));

            if (index >= 0)
            {
                items[index] = new KeyValuePair<TKey, TValue?>(key, value);
            }
            else
            {
                items.Insert(~index, new KeyValuePair<TKey, TValue?>(key, value));
            }
        }
    }

    public void Add(TKey key, TValue value)
    {
        int index = items.BinarySearch(new KeyValuePair<TKey, TValue?>(key, default),
            new KeyValuePairComparer<TKey, TValue?>(comparer));

        if (index < 0)
        {
            index = ~index;
        }

        items.Insert(index, new KeyValuePair<TKey, TValue?>(key, value));
    }

    public void Clear() => items.Clear();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
        items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public int IndexOf(TKey key) =>
        items.FindIndex(kvp => comparer.Compare(kvp.Key, key) == 0);

    public bool Remove(TKey key)
    {
        int index = items.FindIndex(kvp => comparer.Compare(kvp.Key, key) == 0);
        if (index >= 0)
        {
            items.RemoveAt(index);
            return true;
        }
        return false;
    }

    public bool TryGetValue(TKey key, out TValue? value)
    {
        int index = items.BinarySearch(new KeyValuePair<TKey, TValue?>(key, default(TValue)),
            new KeyValuePairComparer<TKey, TValue?>(comparer));

        if (index >= 0)
        {
            value = items[index].Value;
            return true;
        }

        value = default;
        return false;
    }

    public bool Contains(TKey key)
    {
        int index = items.FindIndex(kvp => comparer.Compare(kvp.Key, key) == 0);
        if (index >= 0)
        {
            items.RemoveAt(index);
            return true;
        }
        return false;
    }

    private class KeyValuePairComparer<TK, TV>(IComparer<TK> comparer) :
                                IComparer<KeyValuePair<TK, TV>>
    {
        private readonly IComparer<TK> comparer = comparer ?? Comparer<TK>.Default;

        public int Compare(KeyValuePair<TK, TV> x, KeyValuePair<TK, TV> y) =>
            comparer.Compare(x.Key, y.Key);
    }
}