namespace Toolkit.Foundation;

public record Scoped(string Key)
{
    public override string ToString() => Key;
}