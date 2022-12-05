namespace Toolkit.Foundation
{
    public interface IWritableJsonConfigurationDescriptor
    {
        Type ConfigurationType { get; }

        string Key { get; }
    }

}
