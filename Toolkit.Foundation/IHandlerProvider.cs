
namespace Toolkit.Foundation
{
    public interface IHandlerProvider
    {
        IEnumerable<object?> Get(Type type, 
            object? key = null);
    }
}