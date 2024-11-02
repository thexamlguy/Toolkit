using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class Component :
    IComponent
{
    private readonly IComponentBuilder builder;

    protected Component(IComponentBuilder builder) =>
        this.builder = builder;

    public static TComponent Create<TComponent>(IServiceProvider provider,
        Action<IComponentBuilder> builderDelegate)
        where TComponent : class, IComponent
    {
        IServiceFactory factory = provider.GetRequiredService<IServiceFactory>();

        IComponentBuilder builder = ComponentBuilder.Create();
        builderDelegate(builder);

        return factory.Create<TComponent>(builder);
    }

    public IComponentBuilder Configure(Action<IComponentBuilder>? componentDelegate = null)
    { 
        if (componentDelegate is not null)
        {
            componentDelegate(builder);
        }

        return Configuring(builder);
    }

    public IComponentBuilder Configure<TConfiguration>(Action<IConfigurationBuilder<TConfiguration>>? configurationDelegate = null,
        Action<IComponentBuilder>? componentDelegate = null)
        where TConfiguration : class, new()
    {
        if (componentDelegate is not null)
        {
            componentDelegate(builder);
        }

        if (configurationDelegate is not null)
        {
            ConfigurationBuilder<TConfiguration> configurationBuilder = new();

            configurationDelegate(configurationBuilder);

            if (configurationBuilder.Section is { Length: > 0 } section)
            {
                builder.AddConfiguration(section, configurationBuilder.Configuration ?? new TConfiguration());
            }
        }

        return Configuring(builder);
    }

    public virtual IComponentBuilder Configuring(IComponentBuilder builder) => builder;
}