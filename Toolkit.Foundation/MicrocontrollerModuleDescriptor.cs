namespace Toolkit.Foundation;

public record MicroControllerModuleDescriptor<TModule> : 
    IMicroControllerModuleDescriptor<TModule> where TModule : IMicroControllerModule, new()
{
    public Type Type => typeof(TModule);

    public Func<TModule>? Factory => new(() => new TModule());
}
