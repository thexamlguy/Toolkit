using Avalonia.Media.Imaging;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ImageWriter :
    IImageWriter
{
    public void Write(IImageDescriptor imageDescriptor,
        Stream stream)
    {
        if (imageDescriptor.Image is Bitmap bitmap)
        {
            bitmap.Save(stream);
        }
    }
}