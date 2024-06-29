namespace Toolkit.Foundation;

public interface IImageWriter
{
    void Write(IImageDescriptor imageDescriptor, Stream stream);
}