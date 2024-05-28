namespace Toolkit.Foundation;

public interface IConfigurationDescriptor<out TConfiguration>
    where TConfiguration :
    class
{
    string Name { get; }

    string Section { get; }

    TConfiguration Value { get; }
}