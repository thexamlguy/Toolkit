namespace Toolkit.Foundation;

public interface IMicrocontrollerContext<TRead, TModuleDeserializer> :
    IMicrocontrollerContext where TModuleDeserializer : IMicrocontrollerModuleDeserializer<TRead>, new();

public interface IMicrocontrollerContext;