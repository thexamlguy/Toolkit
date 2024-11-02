namespace Toolkit.Foundation;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class NotificationAttribute(Type type,
    object key) : Attribute
{
    public Type Type => type;

    public object Key => key;
}