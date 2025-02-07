using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Foundation;

public class MicrocontrollerConfiguration : 
    IMicrocontrollerConfiguration
{
    [NotNull]
    public string? PortName { get; set; }

    public int BaudRate { get; set; }
}