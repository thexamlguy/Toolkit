namespace Toolkit.Foundation;

public interface IValueInvoker<TValue>
{
    public void Invoke(TValue args);
}