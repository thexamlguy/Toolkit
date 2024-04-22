namespace Toolkit.Foundation;

public class ComponentScopeProvider(IComponentScopeCollection scopes) :
    IComponentScopeProvider
{
    public ComponentScopeDescriptor? Get(string key)
    {
        return scopes.FirstOrDefault(x => x.Key == key) is ComponentScopeDescriptor
            descriptor ? descriptor : default;
    }
}