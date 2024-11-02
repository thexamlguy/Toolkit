namespace Toolkit.Foundation;

public interface IComponent
{
    IComponentBuilder Configure(Action<IComponentBuilder>? configurationDelegate = null);

    IComponentBuilder Configure<TConfiguration>(Action<IConfigurationBuilder<TConfiguration>>? configurationDelegate = null,
        Action<IComponentBuilder>? componentDelegate = null)
        where TConfiguration : class, new();
}