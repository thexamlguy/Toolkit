namespace Toolkit.Foundation;

public class ConfigurationBuilder<TConfiguration> :
    IConfigurationBuilder<TConfiguration>
    where TConfiguration : class
{
    public string? Section { get; set; }

    public TConfiguration? Configuration { get; set; }
}
