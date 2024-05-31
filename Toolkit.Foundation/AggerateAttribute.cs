namespace Toolkit.Foundation;

public class AggerateAttribute(Type type, object key,
    AggerateMode mode = AggerateMode.Reset) : NotificationAttribute(type, key)
{
    public AggerateMode Mode => mode;
}