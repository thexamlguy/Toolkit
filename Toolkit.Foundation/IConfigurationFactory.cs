namespace Toolkit.Foundation;

public interface IConfigurationFactory<TConfiguration> 
    where TConfiguration : 
    class
{
    object Create();
}