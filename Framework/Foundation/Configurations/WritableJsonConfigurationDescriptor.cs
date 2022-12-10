namespace Toolkit.Framework.Foundation;

public record WritableJsonConfigurationDescriptor(Type ConfigurationType, string Key) : IWritableJsonConfigurationDescriptor;