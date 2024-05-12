using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class ComponentHost(IServiceProvider services,
    IEnumerable<IInitializer> initializers,
    IEnumerable<IHostedService> hostedServices) :
    IComponentHost
{
    public IServiceProvider Services => services;


    public void Dispose()
    {
    }

    public TConfiguration? GetConfiguration<TConfiguration>() where TConfiguration : ComponentConfiguration
    {
       return Services.GetService<TConfiguration>();
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        foreach (IInitializer initializer in initializers)
        {
            await initializer.Initialize();
        }

        foreach (IHostedService service in hostedServices)
        {
            await service.StartAsync(cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        foreach (IHostedService service in hostedServices)
        {
            await service.StopAsync(cancellationToken);
        }
    }
}