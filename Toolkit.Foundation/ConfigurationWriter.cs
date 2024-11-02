namespace Toolkit.Foundation;

public class ConfigurationWriter<TConfiguration>(IConfigurationSource<TConfiguration> source,
    IConfigurationFactory<TConfiguration> factory) :
    IConfigurationWriter<TConfiguration>
    where TConfiguration :
    class
{
    public void Write(Action<TConfiguration> updateDelegate)
    {
        if (!source.TryGet(out TConfiguration? value))
        {
            value = (TConfiguration)factory.Create();
        }

        if (value is not null)
        {
            updateDelegate?.Invoke(value);
            Write(value);
        }
    }

    public void Write(object value) => source.Set(value);

    public void Write(TConfiguration value) => source.Set(value);
}
