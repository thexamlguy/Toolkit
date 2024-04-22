namespace Toolkit.Foundation;

public interface IConfigurationDescriptor<out TConfiguration>
    where TConfiguration :
    class
{
    TConfiguration Value { get; }

    string Section { get; }
}
