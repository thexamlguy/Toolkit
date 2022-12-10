namespace Toolkit.Foundation;

public record WritableJsonConfigurationDescriptor(Type ConfigurationType, string Key) : IWritableJsonConfigurationDescriptor;
