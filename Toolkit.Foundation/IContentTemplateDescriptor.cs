namespace Toolkit.Foundation;

public interface IContentTemplateDescriptor
{
    object Key { get; }

    Type ContentType { get; }

    Type TemplateType { get; }
}