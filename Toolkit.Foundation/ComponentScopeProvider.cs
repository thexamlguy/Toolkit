namespace Toolkit.Foundation;

public class ComponentScopeProvider(IComponentScopeCollection scopes) :
    IComponentScopeProvider
{
    public IServiceProvider? Get(string name)
    {
        return scopes.TryGetValue(name, 
            out IServiceProvider? scope) ? scope : default;
    }
}

