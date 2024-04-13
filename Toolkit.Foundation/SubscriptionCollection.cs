using System.Collections.Concurrent;

namespace Toolkit.Foundation;

public class SubscriptionCollection :
    ConcurrentDictionary<object, List<WeakReference>>;
