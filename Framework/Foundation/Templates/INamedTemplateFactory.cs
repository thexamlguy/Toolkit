namespace Toolkit.Framework.Foundation;

public interface INamedTemplateFactory
{
    object? Create(string name);
}