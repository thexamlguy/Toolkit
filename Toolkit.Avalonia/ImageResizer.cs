using Avalonia.Media.Imaging;
using SkiaSharp;

namespace Toolkit.Avalonia;

public class ImageResizer :
    IImageResizer
{
    public Bitmap Resize(Stream stream,
        double width,
        double height,
        bool maintainAspectRatio)
    {
        Bitmap bitmap = new(stream);

        if (bitmap.Size.Width != width || bitmap.Size.Height != height)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using SKBitmap sKBitmap = SKBitmap.Decode(stream);

            if (height == 0 && maintainAspectRatio)
            {
                height = (int)((float)width / sKBitmap.Width * sKBitmap.Height);
            }

            if (width == 0 && maintainAspectRatio)
            {
                width = (int)((float)height / sKBitmap.Height * sKBitmap.Width);
            }

            float widthRatio = (float)width / sKBitmap.Width;
            float heightRatio = (float)height / sKBitmap.Height;
            float scale = maintainAspectRatio ? Math.Max(widthRatio, heightRatio) : Math.Min(widthRatio, heightRatio);

            int newWidth = (int)(sKBitmap.Width * scale);
            int newHeight = (int)(sKBitmap.Height * scale);

            using SKBitmap resized = new(newWidth, newHeight);
            using SKCanvas canvas = new(resized);

            using SKPaint paint = new()
            {
                FilterQuality = SKFilterQuality.High,
                IsAntialias = true
            };

            canvas.Clear(SKColors.Transparent);
            canvas.DrawBitmap(sKBitmap, new SKRect(0, 0, newWidth, newHeight), paint);

            SKBitmap cropped;
            if (maintainAspectRatio)
            {
                int cropX = (int)(newWidth - width) / 2;
                int cropY = (int)(newHeight - height) / 2;

                cropped = new SKBitmap((int)width, (int)height);
                using SKCanvas croppedCanvas = new(cropped);
                SKRect cropRect = new(cropX, cropY, (int)(cropX + width), cropY + (int)height);
                croppedCanvas.Clear(SKColors.Transparent);
                croppedCanvas.DrawBitmap(resized, cropRect, new SKRect(0, 0, (int)width, (int)height));
            }
            else
            {
                cropped = resized;
            }

            using SKImage image = SKImage.FromBitmap(cropped);
            using MemoryStream outputStream = new();
            image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(outputStream);
            outputStream.Seek(0, SeekOrigin.Begin);
            bitmap.Dispose();

            return new Bitmap(outputStream);
        }

        return bitmap;
    }

}