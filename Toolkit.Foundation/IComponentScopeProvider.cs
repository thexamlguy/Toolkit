namespace Toolkit.Foundation;

public interface IComponentScopeProvider
{
    IServiceProvider? Get(string name);
}

