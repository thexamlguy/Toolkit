namespace Toolkit.Foundation;

public interface IHandlerProvider
{
    IEnumerable<object?> Get(object key);
}