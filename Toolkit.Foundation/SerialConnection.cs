using System.IO.Ports;

namespace Toolkit.Foundation;

public class SerialConnection(SerialPort serialPort) : 
    ISerialConnection
{
    public bool IsOpen { get; private set; }

    public void Close()
    {
        if (IsOpen)
        {
            try
            {
                serialPort.Close();
            }
            catch
            {

            }

            IsOpen = serialPort.IsOpen;
        }
    }

    public bool Open()
    {
        if (!IsOpen)
        {
            try
            {
                serialPort.Open();

                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
            catch
            {

            }

            IsOpen = serialPort.IsOpen;
        }

        return IsOpen;
    }
}
