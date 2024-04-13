using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Styling;

namespace Toolkit.UI.Controls.Avalonia;

public class FastRendererBackground : 
    Image, IDisposable
{
    private const int ImageWidth = 100;
    private const int ImageHeight = 100;

    private readonly WriteableBitmap bitmap = new(new PixelSize(ImageWidth, ImageHeight), 
        new Vector(96, 96), PixelFormat.Bgra8888);

    private readonly FastNoiseBackgroundRenderer renderer = new();

    public FastRendererBackground()
    {
        Source = bitmap;
        Stretch = Stretch.UniformToFill;
    }

    public override void EndInit()
    {
        base.EndInit();
        if (Application.Current?.ActualThemeVariant is ThemeVariant theme)
        {
            renderer.UpdateValues((Color)Application.Current.FindResource("SystemAccentColorLight3"), 
                (Color)Application.Current.FindResource("SystemAccentColorDark3"), theme);
        }

        renderer.Render(bitmap);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        bitmap.Dispose();
    }
}
