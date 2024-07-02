namespace Toolkit.Foundation;

public record Activation
{
    public static ActivationEventArgs<TSender, TValue> As<TSender, TValue>(TValue value) => new(value);

    public static ActivationEventArgs<TSender> As<TSender>() =>
        new();
}
