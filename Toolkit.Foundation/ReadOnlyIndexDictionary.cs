using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Foundation;

public class ReadOnlyIndexDictionary<TKey, TValue> :
    IReadOnlyDictionary<TKey, TValue>, 
    IReadOnlyIndexDictionary<TKey, TValue> 
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> dictionary;
    private readonly List<KeyValuePair<TKey, TValue>> indexedItems;

    public ReadOnlyIndexDictionary(IDictionary<TKey, TValue> items)
    {
        dictionary = new Dictionary<TKey, TValue>(items);
        indexedItems = [.. dictionary];
    }

    public TValue this[TKey key] =>
        dictionary[key];

    public KeyValuePair<TKey, TValue> this[int index] =>
        indexedItems[index];

    public IEnumerable<TKey> Keys =>
        dictionary.Keys;

    public IEnumerable<TValue> Values =>
        dictionary.Values;

    public int Count => dictionary.Count;

    public bool ContainsKey(TKey key) =>
        dictionary.ContainsKey(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
        dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (dictionary.TryGetValue(key, out TValue? localValue))
        {
            if (localValue is not null)
            {
                value = localValue;
                return true;
            }
        }

        value = default;
        return false;
    }
}
