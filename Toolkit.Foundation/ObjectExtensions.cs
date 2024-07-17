using System.Reflection;

namespace Toolkit.Foundation;

public static class ObjectExtensions
{
    public static object? GetPropertyValue(this object obj, Func<object> selector)
    {
        Type type = obj.GetType();

        object? key = selector();
        if (type.GetProperty($"{key}") is PropertyInfo property && property.GetValue(obj) is { } value)
        {
            return value;
        }

        return null;
    }

    public static TAttribute? GetAttribute<TAttribute>(this object obj)
        where TAttribute : Attribute
    {
        Type type = obj.GetType();
        if (type.GetCustomAttribute<TAttribute>(true) is TAttribute attribute)
        {
            return attribute;
        }

        return null;
    }

    public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this object obj)
        where TAttribute : Attribute
    {
        Type type = obj.GetType();
        if (type.GetCustomAttributes<TAttribute>(true) is IEnumerable<TAttribute> attributes)
        {
            return attributes;
        }

        return Enumerable.Empty<TAttribute>();
    }
}