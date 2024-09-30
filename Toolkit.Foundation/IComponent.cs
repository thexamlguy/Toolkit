namespace Toolkit.Foundation;

public interface IComponent
{
    IComponentBuilder Configure(string? name = null,
        Action<IComponentBuilder>? builderDelegate = null);
}