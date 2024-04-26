namespace Toolkit.Foundation;

public record Request<TValue>;

public class Request
{
    public static Request<TValue> As<TValue>() => new();
}