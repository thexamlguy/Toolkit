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

    public IComponentBuilder Configure(string? name = null,
        Action<IComponentBuilder>? builderDelegate = null)
    {
        if (builderDelegate is not null)
        {
            builderDelegate(builder);
        }

        return Configuring(builder);
    }

    public virtual IComponentBuilder Configuring(IComponentBuilder builder) => builder;
}