namespace Toolkit.Avalonia;

public class TransientNavigationStore<TControl> : ITransientNavigationStore<TControl>
    where TControl : class
{
    private readonly Dictionary<TControl, Dictionary<Type, object>> controlDataMap = [];

    public void Set(TControl control, 
        object parameters)
    {
        if (!controlDataMap.TryGetValue(control, out var typeMap))
        {
            typeMap = new Dictionary<Type, object>();
            controlDataMap[control] = typeMap;
        }

        typeMap[parameters.GetType()] = parameters;
    }

    public T? Get<T>(TControl control)
        where T : class
    {
        if (controlDataMap.TryGetValue(control, out var typeMap) &&
            typeMap.TryGetValue(typeof(T), out var parameters))
        {
            typeMap.Remove(typeof(T));
            return parameters as T;
        }

        return default;
    }

    public void Remove(TControl control) => controlDataMap.Remove(control);

    public void Clear() => controlDataMap.Clear();
}
