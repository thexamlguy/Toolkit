namespace Toolkit.Foundation;

public interface IImageReader
{
    IImageDescriptor Get(Stream stream,
        int width,
        int height,
        bool maintainAspectRatio = false);
}