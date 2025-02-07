using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Foundation;

public class SerialConfiguration :
    ISerialConfiguration
{
    [NotNull]
    public string? PortName { get; set; }

    public int BaudRate { get; set; }
}
