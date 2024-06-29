using Avalonia.Media.Imaging;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ImageReader(IImageResizer imageResizer) : 
    IImageReader
{
    public async Task<IImageDescriptor> Get(Stream stream, 
        int width, 
        int height, 
        bool maintainAspectRatio)
    {
        Bitmap resizedImage = imageResizer.Resize(stream, width, height, maintainAspectRatio);
        return new ImageDescriptor(resizedImage, width, height);
    }
}
