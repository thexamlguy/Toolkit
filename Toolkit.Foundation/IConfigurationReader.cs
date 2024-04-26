namespace Toolkit.Foundation;

public interface IConfigurationReader<TConfiguration>
    where TConfiguration :
    class
{
    bool TryRead(out TConfiguration? configuration);

    TConfiguration Read();
}