using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class Component :
    IComponent
{
    private readonly IComponentBuilder builder;

    protected Component(IComponentBuilder builder)
    {
        this.builder = builder;
    }

    public static TComponent? Create<TComponent>(IServiceProvider provider,
        Action<IComponentBuilder> builderDelegate)
        where TComponent : class, IComponent
    {
        if (provider.GetRequiredService<IServiceFactory>() is IServiceFactory factory)
        {
            IComponentBuilder builder = ComponentBuilder.Create();
            builderDelegate(builder);

            return factory.Create<TComponent>(builder);
        }
        return default;
    }

    public virtual IComponentBuilder Create() => builder;
}