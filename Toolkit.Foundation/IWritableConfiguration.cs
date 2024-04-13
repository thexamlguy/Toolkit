namespace Toolkit.Foundation;

public interface IWritableConfiguration<out TConfiguration>
    where TConfiguration :
    class
{
    void Write(Action<TConfiguration> updateDelegate);
}
