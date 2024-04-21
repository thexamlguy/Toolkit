namespace Toolkit.Foundation;

public interface IConfiguration<out TConfiguration>
    where TConfiguration :
    class
{
    TConfiguration Value { get; }

    string Section { get; }
}
