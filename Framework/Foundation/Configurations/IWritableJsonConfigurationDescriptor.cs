namespace Toolkit.Framework.Foundation;

public interface IWritableJsonConfigurationDescriptor
{
    Type ConfigurationType { get; }

    string Key { get; }
}