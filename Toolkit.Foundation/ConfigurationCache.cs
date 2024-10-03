using System.Collections.Concurrent;

namespace Toolkit.Foundation;

public static class ConfigurationCache
{
    private static readonly ConcurrentDictionary<string, object?> cache = new();

    public static void Set<TConfiguration>(string section,
        TConfiguration configuration) => cache[section] = configuration;

    public static bool Remove(string section) => cache.TryRemove(section, out _);

    public static bool TryGet<TConfiguration>(string section,
        out TConfiguration? configuration)
    {
        if (cache.TryGetValue(section, out object? cachedValue))
        {
            configuration = (TConfiguration?)cachedValue;
            return true;
        }

        configuration = default;
        return false;
    }
}
