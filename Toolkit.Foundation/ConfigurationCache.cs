using System.Collections.Concurrent;

namespace Toolkit.Foundation;

public class ConfigurationCache : 
    IConfigurationCache
{
    private readonly ConcurrentDictionary<string, object?> cache = new();

    public void Set<TConfiguration>(string section,
        TConfiguration configuration) => cache[section] = configuration;

    public bool Remove(string section) => cache.TryRemove(section, out _);

    public bool TryGet<TConfiguration>(string section,
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
