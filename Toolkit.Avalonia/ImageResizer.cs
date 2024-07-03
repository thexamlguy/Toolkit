using Avalonia.Media.Imaging;
using SkiaSharp;

namespace Toolkit.Avalonia;

public class ImageResizer :
    IImageResizer
{
    public Bitmap Resize(Stream stream,
        int targetWidth,
        int targetHeight,
        bool maintainAspectRatio)
    {
        Bitmap bitmap = new(stream);

        if (bitmap.Size.Width != targetWidth || bitmap.Size.Height != targetHeight)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using SKBitmap sKBitmap = SKBitmap.Decode(stream);

            float widthRatio = (float)targetWidth / sKBitmap.Width;
            float heightRatio = (float)targetHeight / sKBitmap.Height;
            float scale = maintainAspectRatio ? Math.Max(widthRatio, heightRatio) : Math.Min(widthRatio, heightRatio);

            int newWidth = (int)(sKBitmap.Width * scale);
            int newHeight = (int)(sKBitmap.Height * scale);

            using SKBitmap resized = new(newWidth, newHeight);
            using SKCanvas canvas = new(resized);

            canvas.Clear(SKColors.Transparent);
            canvas.DrawBitmap(sKBitmap, new SKRect(0, 0, newWidth, newHeight));

            SKBitmap cropped;
            if (maintainAspectRatio)
            {
                int cropX = (newWidth - targetWidth) / 2;
                int cropY = (newHeight - targetHeight) / 2;

                cropped = new SKBitmap(targetWidth, targetHeight);
                using SKCanvas croppedCanvas = new(cropped);
                SKRect cropRect = new(cropX, cropY, cropX + targetWidth, cropY + targetHeight);
                croppedCanvas.Clear(SKColors.Transparent);
                croppedCanvas.DrawBitmap(resized, cropRect, new SKRect(0, 0, targetWidth, targetHeight));
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