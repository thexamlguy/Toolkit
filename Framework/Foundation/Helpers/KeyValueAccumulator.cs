using Microsoft.Extensions.Primitives;

namespace Toolkit.Framework.Foundation;

public struct KeyValueAccumulator
{
    private Dictionary<string, StringValues> accumulator;
    private Dictionary<string, List<string>> expandingAccumulator;

    public void Append(string key, string value)
    {
        accumulator ??= new Dictionary<string, StringValues>(StringComparer.OrdinalIgnoreCase);

        if (accumulator.TryGetValue(key, out StringValues values))
        {
            if (values.Count == 0)
            {
                expandingAccumulator[key].Add(value);
            }
            else if (values.Count == 1)
            {
                accumulator[key] = new string[] { values[0]!, value };
            }
            else
            {
                accumulator[key] = default;

                expandingAccumulator ??= new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

                List<string> list = new(8);
                string?[] array = values.ToArray();

                list.Add(array[0]!);
                list.Add(array[1]!);
                list.Add(value);

                expandingAccumulator[key] = list;
            }
        }
        else
        {
            accumulator[key] = new StringValues(value);
        }

        ValueCount++;
    }

    public bool HasValues => ValueCount > 0;

    public int KeyCount => accumulator?.Count ?? 0;

    public int ValueCount { get; private set; }

    public Dictionary<string, StringValues> GetResults()
    {
        if (expandingAccumulator != null)
        {
            foreach (var entry in expandingAccumulator)
            {
                accumulator[entry.Key] = new StringValues(entry.Value.ToArray());
            }
        }

        return accumulator ?? new Dictionary<string, StringValues>(0, StringComparer.OrdinalIgnoreCase);
    }
}