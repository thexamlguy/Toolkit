namespace Toolkit.Foundation;

public record NamedComponent(string Key)
{
    public override string ToString() => Key;
}