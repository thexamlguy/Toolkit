namespace Toolkit.Foundation;

public class ComponentScopeCollection : Dictionary<string, IServiceProvider>, 
    IComponentScopeCollection;
