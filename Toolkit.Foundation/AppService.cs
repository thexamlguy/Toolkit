using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class AppService(IEnumerable<IInitialization> initializations,
    IEnumerable<IAsyncInitialization> asyncInitializations,
    IPublisher publisher) :
    IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (IInitialization initialization in initializations)
        {
            initialization.Initialize();
        }

        foreach (IAsyncInitialization initialization in asyncInitializations)
        {
            await initialization.Initialize();
        }

        publisher.Publish<StartedEventArgs>();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}