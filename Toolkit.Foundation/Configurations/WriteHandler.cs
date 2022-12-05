using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation
{
    public class WriteHandler<TConfiguration> : IRecipient<Write<TConfiguration>> where TConfiguration : class
    {
        private readonly IMessenger messenger;
        private readonly TConfiguration configuration;
        private readonly IConfigurationWriter<TConfiguration> writer;

        public WriteHandler(TConfiguration configuration,
            IConfigurationWriter<TConfiguration> writer,
            IMessenger messenger)
        {
            this.messenger = messenger;
            this.configuration = configuration;
            this.writer = writer;
        }

        public void Receive(Write<TConfiguration> request)
        {
            request.UpdateDelegate.Invoke(configuration);
            writer.Write(request.Section, configuration);

            messenger.Send(new ConfigurationChanged<TConfiguration>(configuration));
        }
    }
}
