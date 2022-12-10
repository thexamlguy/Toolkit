using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class ConfigurationWriter<TConfiguration> : IConfigurationWriter<TConfiguration> where TConfiguration : class, new()
{
    private readonly IConfiguration rootConfiguration;

    public ConfigurationWriter(IConfiguration rootConfiguration)
    {
        this.rootConfiguration = rootConfiguration;
    }

    public void Write(string section, TConfiguration configuration)
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
