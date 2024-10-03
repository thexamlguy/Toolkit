namespace Toolkit.Foundation;

public interface IComponent
{
    IComponentBuilder Configure(string name,
        Action<IComponentBuilder>? builderDelegate = null);

    IComponentBuilder Configure(Action<IComponentBuilder>? builderDelegate = null);

    IComponentBuilder Configure<TConfiguration>(string name,
        TConfiguration? configuration = null,
        Action<IComponentBuilder>? builderDelegate = null)
        where TConfiguration : class, new();
}