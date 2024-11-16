using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class AppService(IEnumerable<IInitialization> initializations,
    IEnumerable<IAsyncInitialization> asyncInitializations,
    IMessenger messenger) :
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
        
        messenger.Send<StartedEventArgs>();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}