namespace Toolkit.Foundation;

public record ActivationEventArgs<TSynchronize, TValue>(TValue? Value = default) :
    IActivation;

public record ActivationEventArgs<TSynchronize>() :
    IActivation;