namespace Toolkit.Foundation;

public interface ICache<TValue> :
    IEnumerable<TValue>
{
    void Add(TValue value);

    void Clear();

    bool Contains(TValue key);

    int IndexOf(TValue value);

    bool Remove(TValue value);

    bool TryGetValue(TValue key, out TValue? item);
}

public interface ICache<TKey, TValue> :
    IEnumerable<KeyValuePair<TKey, TValue>>
    where TKey :
    notnull
    where TValue :
    notnull
{
    void Add(TKey key, TValue value);

    void Clear();

    bool Contains(TKey key);

    int IndexOf(TKey key);

    bool Remove(TKey key);

    bool TryGetValue(TKey key, out TValue? value);
}