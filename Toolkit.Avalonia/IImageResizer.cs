using Avalonia.Media.Imaging;

namespace Toolkit.Avalonia;

public interface IImageResizer
{
    public Bitmap Resize(Stream stream,
        double width,
        double height,
        bool maintainAspectRatio);
}