namespace Toolkit.Foundation;

public record MicrocontrollerModuleDescriptor<TModule> : 
    IMicrocontrollerModuleDescriptor<TModule> where TModule : IMicrocontrollerModule, new()
{
    public Type Type => typeof(TModule);

    public Func<TModule>? Factory => new(() => new TModule());
}
