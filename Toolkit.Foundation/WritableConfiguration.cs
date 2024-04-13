namespace Toolkit.Foundation;

public class WritableConfiguration<TConfiguration>(IConfigurationWriter<TConfiguration> writer) :
    IWritableConfiguration<TConfiguration>
    where TConfiguration :
    class
{
    public void Write(Action<TConfiguration> updateDelegate) => writer.Write(updateDelegate);
}
