using Avalonia.Media.Imaging;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ImageReader(IImageResizer imageResizer) :
    IImageReader
{
    public IImageDescriptor Get(Stream stream,
        double width,
        double height,
        bool maintainAspectRatio)
    {
        Bitmap resizedImage = imageResizer.Resize(stream,
            width, 
            height,
            maintainAspectRatio);

        return new ImageDescriptor(resizedImage, width, height);
    }

    public IImageDescriptor Get(Stream stream)
    {
        Bitmap image = new(stream);
        return new ImageDescriptor(image,
            image.Size.Width,
            image.Size.Height);
    }
}
