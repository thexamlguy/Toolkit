namespace Toolkit.Foundation;

public class Configuration<TConfiguration>(IConfigurationReader<TConfiguration> reader) :
    IConfiguration<TConfiguration>
    where TConfiguration :
    class
{
    public TConfiguration Value => reader.Read();
}
