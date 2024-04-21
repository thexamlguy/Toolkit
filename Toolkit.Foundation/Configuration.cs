namespace Toolkit.Foundation;

public class Configuration<TConfiguration>(string section,
    IConfigurationReader<TConfiguration> reader) :
    IConfiguration<TConfiguration>
    where TConfiguration :
    class
{
    public TConfiguration Value => reader.Read();

    public string Section => section;
}
