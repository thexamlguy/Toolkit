using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;
using Windows.Storage;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Toolkit.WinUI;

public static class ImageSourceExtensions
{
    public static Icon? ConvertToIcon(this Stream stream, uint dpi)
    {
        return ExtractIcon(dpi, stream);
    }

    private static Icon? ExtractIcon(uint dpi, Stream stream)
    {
        Bitmap bitmap = (Bitmap)Image.FromStream(stream);
        Icon icon = Icon.FromHandle(bitmap.GetHicon());

        return new Icon(icon, new Size(PInvoke.GetSystemMetricsForDpi(SYSTEM_METRICS_INDEX.SM_CXICON, dpi),
            PInvoke.GetSystemMetricsForDpi(SYSTEM_METRICS_INDEX.SM_CYICON, dpi)));
    }
}
