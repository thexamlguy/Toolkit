using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Foundation;

public class ValidationErrorCollection :
    IDictionary<string, string>,
    IDictionary,
    INotifyCollectionChanged,
    INotifyPropertyChanged
{
    private Dictionary<string, string> items;

    public ValidationErrorCollection()
    {
        items = [];
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int Count => items.Count;

    bool IDictionary.IsFixedSize => ((IDictionary)items).IsFixedSize;

    public bool IsReadOnly => false;

    bool ICollection.IsSynchronized => ((IDictionary)items).IsSynchronized;

    public ICollection<string> Keys => items.Keys;

    ICollection IDictionary.Keys => ((IDictionary)items).Keys;

    object ICollection.SyncRoot => ((IDictionary)items).SyncRoot;

    public ICollection<string> Values => items.Values;

    ICollection IDictionary.Values => ((IDictionary)items).Values;

    public string this[string key]
    {
        get => items.ContainsKey(key) ? items[key] : "";
        set
        {
            bool replace = items.TryGetValue(key, out var old);
            items[key] = value;

            if (replace)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs($"Item[{key}]"));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                    new KeyValuePair<string, string>(key, value), new KeyValuePair<string, string>(key, old!)));
            }
            else
            {
                NotifyAdd(key, value);
            }
        }
    }

    object? IDictionary.this[object key] 
    { 
        get => ((IDictionary)items)[key];
        set => ((IDictionary)items)[key] = value; 
    }

    public void Add(string key, string value)
    {
        items.Add(key, value);
        NotifyAdd(key, value);
    }

    void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item) => 
        Add(item.Key, item.Value);

    void IDictionary.Add(object key, object? value) =>
        Add((string)key, (string)value!);

    public void Clear()
    {
        Dictionary<string, string> old = items;
        items = [];

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, old.ToArray(), -1));
    }

    bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item) =>
        items.Contains(item);

    bool IDictionary.Contains(object key) => 
        ((IDictionary)items).Contains(key);

    public bool ContainsKey(string key) => items.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => 
        ((IDictionary<string, string>)items).CopyTo(array, arrayIndex);

    void ICollection.CopyTo(Array array, int index) => 
        ((ICollection)items).CopyTo(array, index);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => 
        items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
        items.GetEnumerator();

    IDictionaryEnumerator IDictionary.GetEnumerator() => 
        ((IDictionary)items).GetEnumerator();

    public bool Remove(string key)
    {
        if (items.TryGetValue(key, out var value))
        {
            items.Remove(key);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs($"Item[{key}]"));

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] { new KeyValuePair<string, string>(key, value) }, -1));

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Contains(string key) => 
        items.ContainsKey(key);

    bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item) => 
        Remove(item.Key);

    void IDictionary.Remove(object key) => Remove((string)key);

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value) => items.TryGetValue(key, out value);

    private void NotifyAdd(string key, string value)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs($"Item[{key}]"));

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, 
            new[] { new KeyValuePair<string, string>(key, value) }, -1));
    }
}
