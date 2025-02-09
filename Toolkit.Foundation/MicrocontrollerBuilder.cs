using System.Collections.ObjectModel;

namespace Toolkit.Foundation;

public class MicroControllerBuilder :
    IMicroControllerBuilder
{
    private readonly List<IMicroControllerBuilderConfiguration> configurations = [];

    public IReadOnlyCollection<IMicroControllerBuilderConfiguration> Configurations => 
        new ReadOnlyCollection<IMicroControllerBuilderConfiguration>(configurations);

    public IMicroControllerBuilderConfiguration<TConfiguration, TReader, TRead, TEvent> Add<TConfiguration, TReader, TRead, TEvent>()
        where TConfiguration : ISerialConfiguration
        where TReader : SerialReader<TRead>
        where TEvent : ISerialEventArgs<TRead>
    {
        MicroControllerBuilderConfiguration<TConfiguration, TReader, TRead, TEvent>? builderConfiguration = new();
        configurations.Add(builderConfiguration);

        return builderConfiguration;
    }
}