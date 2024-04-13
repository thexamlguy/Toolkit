using System.Reflection;

namespace Toolkit.Foundation;

public static class TypeExtensions
{
    public static TAttribute? GetAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute
    {
        if (type.GetCustomAttribute<TAttribute>() is TAttribute attribute)
        {
            return attribute;
        }

        return null;
    }
}
