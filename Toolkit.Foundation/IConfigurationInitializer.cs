namespace Toolkit.Foundation;

public interface IConfigurationInitializer<TConfiguration>
    where TConfiguration :
    class
{
    void Initialize();
}