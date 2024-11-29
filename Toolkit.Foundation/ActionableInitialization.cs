namespace Toolkit.Foundation;

public class ActionableInitialization(IServiceProvider provider, 
    Action<IServiceProvider> delegateAction) : IInitialization
{
    public void Initialize() => delegateAction.Invoke(provider);
}