using System.IO.Ports;

namespace Toolkit.Foundation;

public class SerialStreamer(SerialPort serialPort) : 
    ISerialStreamer
{
    public Stream Create() => new SerialPortStream(serialPort);
}
