namespace Toolkit.Foundation;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class NotificationAttribute(object key) : Attribute
{
    public object Key => key;
}

public class EnumerateAttribute(object key,
    EnumerateMode mode = EnumerateMode.Reset) : NotificationAttribute(key)
{
    public EnumerateMode Mode => mode;
}