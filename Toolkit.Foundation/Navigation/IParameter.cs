using System.Collections.Generic;

namespace Toolkit.Foundation
{
    public interface IParameter
    {
        string? Key { get; }

        KeyValuePair<string, object>? GetValue(object target);
    }
}