using Avalonia.Media.Imaging;

namespace Toolkit.Avalonia;

public interface IImageResizer
{
    public Bitmap Resize(Stream stream,
        int targetWidth,
        int targetHeight,
        bool maintainAspectRatio);
}
