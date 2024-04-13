namespace Toolkit.Foundation;

public class ConfigurationReader<TConfiguration>(IConfigurationSource<TConfiguration> source,
    IConfigurationFactory<TConfiguration> factory) :
    IConfigurationReader<TConfiguration>
    where TConfiguration :
    class
{
    public TConfiguration Read()
    {
        if (source.TryGet(out TConfiguration? value))
        {
            if (value is TConfiguration configuration)
            {
                return configuration;
            }
        }

        return (TConfiguration)factory.Create();
    }

    public bool TryRead(out TConfiguration? configuration)
    {
        if (source.TryGet(out TConfiguration? value) && value is not null)
        {
            configuration = value;
            return true;
        }

        configuration = default;
        return false;
    }
}
