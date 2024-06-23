namespace Toolkit.Foundation;

public record ContentRootConfiguration
{
    public string ContentRoot { get; set; } = "Local";

    public string JsonFileName { get; set; } = "Settings.json";
}