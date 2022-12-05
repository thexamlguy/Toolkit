using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation
{
    public class AppService : IHostedService
    {
        private readonly IMessenger messenger;
        private readonly IInitialization initialization;

        public AppService(IMessenger messenger,
            IInitialization initialization)
        {
            this.messenger = messenger;
            this.initialization = initialization;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            messenger.Send(new Initialize());
            await initialization.InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
