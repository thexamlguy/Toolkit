using System.Collections;

namespace Toolkit.Foundation;

public class Cache<TValue>(IComparer<TValue> comparer) :
    ICache<TValue>
{
    private readonly List<TValue> items = [];

    public IEnumerable<TValue> Items =>
        items;

    public TValue this[int index] =>
        items[index];

    public void Add(TValue item)
    {
        int index = FindInsertIndex(item);
        items.Insert(index, item);
    }

    public void Clear() =>
        items.Clear();

    public bool Contains(TValue item) =>
        items.IndexOf(item) >= 0;

    public IEnumerator<TValue> GetEnumerator() =>
        items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        items.GetEnumerator();

    public int IndexOf(TValue item) =>
        items.IndexOf(item);

    public bool Remove(TValue item) =>
        items.Remove(item);

    public bool TryGetValue(TValue key, 
        out TValue? item)
    {
        var index = items.IndexOf(key);
        item = index >= 0 ? items[index] : default;
        return index >= 0;
    }

    private int FindInsertIndex(TValue item)
    {
        int low = 0, high = items.Count - 1;

        while (low <= high)
        {
            int mid = (low + high) / 2;
            int cmp = comparer.Compare(items[mid], item);

            if (cmp < 0)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        while (low < items.Count && comparer.Compare(items[low], item) == 0)
        {
            low++;
        }

        return low;
    }
}

public class Cache<TKey, TValue>(IComparer<TKey>? comparer = null) : 
    ICache<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    private readonly IComparer<TKey> comparer = comparer ?? Comparer<TKey>.Default;
    private readonly List<KeyValuePair<TKey, TValue?>> items = [];

    public TValue? this[TKey key]
    {
        get
        {
            int index = items.BinarySearch(
                new KeyValuePair<TKey, TValue?>(key, default),
                new KeyValuePairComparer<TKey, TValue?>(comparer));

            if (index >= 0)
            {
                return items[index].Value;
            }

            return default;
        }
        set
        {
            int index = items.BinarySearch(
                new KeyValuePair<TKey, TValue?>(key, default),
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
        int index = items.BinarySearch(
            new KeyValuePair<TKey, TValue?>(key, default),
            new KeyValuePairComparer<TKey, TValue?>(comparer));

        if (index < 0)
        {
            index = ~index;
        }

        items.Insert(index, new KeyValuePair<TKey, TValue?>(key, value));
    }

    public void Clear() => items.Clear();

    public bool Contains(TKey key) =>
        items.FindIndex(kvp => comparer.Compare(kvp.Key, key) == 0) >= 0;

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => 
        items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public int IndexOf(TKey key) => 
        items.FindIndex(kvp => comparer.Compare(kvp.Key, key) == 0);

    public bool Remove(TKey key) => 
        items.RemoveAll(kvp => comparer.Compare(kvp.Key, key) == 0) > 0;

    public bool TryGetValue(TKey key, out TValue? value) =>
        (items.BinarySearch(new KeyValuePair<TKey, TValue?>(key, default),
            new KeyValuePairComparer<TKey, TValue?>(comparer)) is int index && index >= 0)
        ? (value = items[index].Value, true).Item2
        : (value = default, false).Item2;

    private class KeyValuePairComparer<TK, TV>(IComparer<TK> comparer) : 
        IComparer<KeyValuePair<TK, TV>>
    {
        private readonly IComparer<TK> comparer = comparer ?? Comparer<TK>.Default;

        public int Compare(KeyValuePair<TK, TV> x, KeyValuePair<TK, TV> y) =>
            comparer.Compare(x.Key, y.Key);
    }
}