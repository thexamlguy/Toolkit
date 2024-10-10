namespace Toolkit.Foundation;

public interface IImageReader
{
    IImageDescriptor Get(Stream stream,
        double width,
        double height,
        bool maintainAspectRatio = false);

    IImageDescriptor Get(Stream stream);
}