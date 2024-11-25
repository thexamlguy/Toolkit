using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public class ConfigurationInitializer<TConfiguration>(IConfigurationReader<TConfiguration> reader,
    IConfigurationWriter<TConfiguration> writer,
    IConfigurationFactory<TConfiguration> factory,
    IMessenger messenger) :
    IConfigurationInitializer<TConfiguration>,
    IInitialization
    where TConfiguration :
    class
{
    public void Initialize()
    {
        if (!reader.TryRead(out TConfiguration? configuration))
        {
            if (factory.Create() is object defaultConfiguration)
            {
                configuration = (TConfiguration?)defaultConfiguration;
                writer.Write(defaultConfiguration);
            }
        }

        messenger.Send(new ActivatedEventArgs<TConfiguration?>(configuration));
    }
}