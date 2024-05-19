namespace Toolkit.Foundation;

public class AggerateAttribute(object key,
    AggerateMode mode = AggerateMode.Reset) : NotificationAttribute(key)
{
    public AggerateMode Mode => mode;
}