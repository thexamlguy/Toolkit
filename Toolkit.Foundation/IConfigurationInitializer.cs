namespace Toolkit.Foundation;

public interface IConfigurationInitializer<TConfiguration>
    where TConfiguration :
    class
{
    Task Initialize();
}