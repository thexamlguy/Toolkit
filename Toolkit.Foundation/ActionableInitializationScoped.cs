namespace Toolkit.Foundation;

public class ActionableInitializationScoped(IServiceProvider provider,
    Action<IServiceProvider> delegateAction) : IInitializationScoped
{
    public void Initialize() => delegateAction.Invoke(provider);
}