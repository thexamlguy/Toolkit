namespace Toolkit.Foundation;

public record Navigation : 
    INavigation
{
    public required Type Type { get; set; }
}

