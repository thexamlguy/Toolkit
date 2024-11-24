namespace Toolkit.Foundation;

public class ContentTemplateDescriptor(object key,
    Type viewModelType,
    Type viewType,
    params object?[]? parameters) :
    IContentTemplateDescriptor
{
    public object Key => key;

    public object?[]? Parameters => parameters;

    public Type ContentType => viewModelType;

    public Type TemplateType => viewType;
}