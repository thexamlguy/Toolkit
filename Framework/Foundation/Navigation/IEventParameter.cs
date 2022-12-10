using System;
using System.Collections.Generic;

namespace Toolkit.Foundation
{
    public interface IEventParameter
    {
        List<object> GetValues(EventArgs args);
    }
}