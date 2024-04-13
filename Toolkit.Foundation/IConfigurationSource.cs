namespace Toolkit.Foundation;

public interface IConfigurationSource<TConfiguration>
    where TConfiguration :
    class
{
    bool TryGet(out TConfiguration? value);

    void Set(TConfiguration value);

    void Set(object value);
}
