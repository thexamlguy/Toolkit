namespace Toolkit.Foundation;

public interface IComponentScopeProvider
{
    ComponentScopeDescriptor? Get(string key);
}