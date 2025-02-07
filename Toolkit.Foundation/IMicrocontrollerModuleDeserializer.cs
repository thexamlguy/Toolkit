namespace Toolkit.Foundation;

public interface IMicrocontrollerModuleDeserializer<TRead>
{
    public TRead? Read { get; set; }
}
