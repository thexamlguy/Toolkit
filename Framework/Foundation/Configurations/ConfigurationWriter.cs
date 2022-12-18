using Microsoft.Extensions.Configuration;

namespace Toolkit.Framework.Foundation;

public class ConfigurationWriter<TConfiguration> : IConfigurationWriter<TConfiguration> where TConfiguration : class, new()
{
    private readonly IConfiguration rootConfiguration;
    private readonly string section;

    public ConfigurationWriter(IConfiguration rootConfiguration, string section)
    {
        this.rootConfiguration = rootConfiguration;
        this.section = section;
    }

    public void Write(TConfiguration configuration)
    {
        if (rootConfiguration is IConfigurationRoot root)
        {
            foreach (IConfigurationProvider? provider in root.Providers)
            {
                if (provider is IWritableConfigurationProvider writableConfigurationProvider)
                {
                    writableConfigurationProvider.Write(section, configuration);
                }
            }
        }
    }
}