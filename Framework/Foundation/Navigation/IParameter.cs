namespace Toolkit.Framework.Foundation;

public interface IParameter
{
    string? Key { get; }

    KeyValuePair<string, object>? GetValue(object target);
}