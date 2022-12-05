using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation
{
    public class ConfigurationInitializer<TConfiguration> : IInitializer where TConfiguration : class, new()
    {
        private readonly TConfiguration configuration;
        private readonly IMessenger messenger;

        public ConfigurationInitializer(TConfiguration configuration,
            IMessenger messenger)
        {
            this.configuration = configuration;
            this.messenger = messenger;
        }

        public async Task InitializeAsync()
        {
            messenger.Send(configuration);
            await Task.CompletedTask;
        }
    }
}
