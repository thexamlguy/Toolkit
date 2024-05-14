using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class AppService(IEnumerable<IInitializer> initializers,
    IPublisher publisher) :
    IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (IInitializer initializer in initializers)
        {
            await initializer.Initialize();
        }

        publisher.Publish<StartedEventArgs>(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}