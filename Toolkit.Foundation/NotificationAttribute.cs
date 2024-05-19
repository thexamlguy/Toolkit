namespace Toolkit.Foundation;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class NotificationAttribute(object key) : Attribute
{
    public object Key => key;
}

public class EnumerateAttribute(object key,
    AggerateMode mode = AggerateMode.Reset) : NotificationAttribute(key)
{
    public AggerateMode Mode => mode;
}