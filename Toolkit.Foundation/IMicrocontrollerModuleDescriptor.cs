namespace Toolkit.Foundation;

public interface IMicrocontrollerModuleDescriptor<TModule> : 
    IMicrocontrollerModuleDescriptor where TModule : IMicrocontrollerModule
{
    Func<TModule>? Factory { get; }
}

public interface IMicrocontrollerModuleDescriptor
{
    Type Type { get; }
}
