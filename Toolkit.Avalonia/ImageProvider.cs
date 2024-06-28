using Avalonia.Media.Imaging;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ImageProvider(IImageResizer imageResizer) : 
    IImageProvider
{
    public async Task<IImageDescriptor> Get(string filePath, 
        int width, 
        int height, 
        bool maintainAspectRatio)
    {
        await using FileStream stream = File.OpenRead(filePath);
        Bitmap resizedImage = imageResizer.Resize(stream, width, height, maintainAspectRatio);

        return new ImageDescriptor(resizedImage, width, height);
    }
}
