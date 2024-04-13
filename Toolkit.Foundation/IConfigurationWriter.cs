namespace Toolkit.Foundation;

public interface IConfigurationWriter<TConfiguration>
    where TConfiguration :
    class
{
    void Write(Action<TConfiguration> updateDelegate);

    void Write(object value);

    void Write(TConfiguration value);
}