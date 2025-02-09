namespace Toolkit.Foundation;

public interface IMicroControllerModuleDescriptor<TModule> : 
    IMicroControllerModuleDescriptor 
    where TModule : IMicroControllerModule
{
    Func<TModule>? Factory { get; }
}

public interface IMicroControllerModuleDescriptor
{
    Type Type { get; }
}
