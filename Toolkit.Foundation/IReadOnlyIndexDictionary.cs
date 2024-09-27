
namespace Toolkit.Foundation
{
    public interface IReadOnlyIndexDictionary<TKey, TValue> : 
        IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        KeyValuePair<TKey, TValue> this[int index] { get; }
    }
}