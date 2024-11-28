namespace Toolkit.Foundation;

public static class TypeExtensions
{
    public static Type MakeNullable(this Type type) => 
        type.IsValueType && Nullable.GetUnderlyingType(type) == null
            ? typeof(Nullable<>).MakeGenericType(type)
            : type;
}
