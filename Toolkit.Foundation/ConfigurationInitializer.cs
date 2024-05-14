namespace Toolkit.Foundation;

public class ConfigurationInitializer<TConfiguration>(IConfigurationReader<TConfiguration> reader,
    IConfigurationWriter<TConfiguration> writer,
    IConfigurationFactory<TConfiguration> factory,
    IPublisher publisher) :
    IConfigurationInitializer<TConfiguration>,
    IInitializer
    where TConfiguration :
    class
{
    public Task Initialize()
    {
        if (!reader.TryRead(out TConfiguration? configuration))
        {
            if (factory.Create() is object defaultConfiguration)
            {
                configuration = (TConfiguration?)defaultConfiguration;
                writer.Write(defaultConfiguration);
            }
        }

        publisher.PublishUI(new ActivatedEventArgs<TConfiguration>(configuration));
        return Task.CompletedTask;
    }
}