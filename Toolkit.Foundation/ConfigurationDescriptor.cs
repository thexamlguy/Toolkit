namespace Toolkit.Foundation;

public class ConfigurationDescriptor<TConfiguration>(string section,
    IConfigurationReader<TConfiguration> reader) :
    IConfigurationDescriptor<TConfiguration>
    where TConfiguration :
    class
{
    public TConfiguration Value => reader.Read();

    public string Section => section;
}
