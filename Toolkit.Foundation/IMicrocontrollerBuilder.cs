namespace Toolkit.Foundation;

public interface IMicroControllerBuilder
{
    IReadOnlyCollection<IMicroControllerBuilderConfiguration> Configurations { get; }

    IMicroControllerBuilderConfiguration<TConfiguration, TReader, TRead, TEvent> Add<TConfiguration, TReader, TRead, TEvent>()
        where TConfiguration : ISerialConfiguration
        where TReader : SerialReader<TRead> 
        where TEvent : ISerialEventArgs<TRead>;
}
