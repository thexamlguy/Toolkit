namespace Toolkit.Framework.Foundation;

public interface IEventParameter
{
    List<object> GetValues(EventArgs args);
}