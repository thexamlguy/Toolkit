namespace Toolkit.Foundation;

public interface IMicroControllerBuilderConfiguration
{
    IReadOnlyCollection<IMicroControllerModuleDescriptor> Modules { get; }

    Func<IServiceProvider, IMicroControllerContext?> Factory { get; }
}

public interface IMicroControllerBuilderConfiguration<TConfiguration, TReader, TRead, IEvent> :
    IMicroControllerBuilderConfiguration
    where TConfiguration : ISerialConfiguration
    where TReader : SerialReader<TRead>
    where IEvent : ISerialEventArgs<TRead>
{

    IMicroControllerBuilderConfiguration<TConfiguration, TReader, TRead, IEvent> AddModule<TModule>()
        where TModule : IMicroControllerModule, new();
}
