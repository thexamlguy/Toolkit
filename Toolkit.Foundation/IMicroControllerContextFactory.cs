namespace Toolkit.Foundation;

public interface IMicroControllerContextFactory
{
    IMicroControllerContext<TRead, TEvent>? Create<TConfiguration, TReader, TRead, TEvent>(IReadOnlyCollection<IMicroControllerModuleDescriptor> modules)
        where TConfiguration : ISerialConfiguration
        where TReader : SerialReader<TRead>
        where TEvent : ISerialEventArgs<TRead>;
}
