
namespace Toolkit.Foundation;

public record Activated
{
    public static ActivatedEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static ActivatedEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}