namespace Toolkit.Foundation;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class NotificationAttribute(object key) : Attribute
{
    public object Key => key;
}
