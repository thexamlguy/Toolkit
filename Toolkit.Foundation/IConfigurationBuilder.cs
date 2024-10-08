namespace Toolkit.Foundation;

public interface IConfigurationBuilder<TConfiguration>
{
    string? Section { get; set; }

    TConfiguration? Configuration { get; set; }
}
