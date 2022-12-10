using System.Collections.ObjectModel;

namespace Toolkit.Framework.Avalonia;

public class TriggerCollection : Collection<Delegate>
{
    public void Add(object item)
    {
        if (item is Delegate trigger)
        {
            base.Add(trigger);
        }
    }
}