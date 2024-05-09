namespace Toolkit.Foundation;

public record NamedComponent(string Name)
{
    public override string ToString() => Name;
}