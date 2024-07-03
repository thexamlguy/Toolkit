using Avalonia.Media.Imaging;
using SkiaSharp;

namespace Toolkit.Avalonia;

public class ImageResizer : 
    IImageResizer
{
    public Bitmap Resize(Stream stream, int targetWidth, int targetHeight, bool maintainAspectRatio)
    {
        Bitmap bitmap = new(stream);

        if (bitmap.Size.Width != targetWidth || bitmap.Size.Height != targetHeight)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using SKBitmap sKBitmap = SKBitmap.Decode(stream);
            float widthRatio = (float)targetWidth / sKBitmap.Width;
            float heightRatio = (float)targetHeight / sKBitmap.Height;
            float scale = maintainAspectRatio ? Math.Min(widthRatio, heightRatio) : Math.Max(widthRatio, heightRatio);

            int newWidth = (int)(sKBitmap.Width * scale);
            int newHeight = (int)(sKBitmap.Height * scale);

            using SKBitmap resized = sKBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High);

            SKBitmap cropped = resized;
            if (maintainAspectRatio)
            {
                int cropX = (resized.Width - targetWidth) / 2;
                int cropY = (resized.Height - targetHeight) / 2;

                cropped = new SKBitmap(targetWidth, targetHeight);
                using SKCanvas croppedCanvas = new(cropped);
                SKRect originalRect = new(cropX, cropY, cropX + targetWidth, cropY + targetHeight);
                SKRect targetRect = new(0, 0, targetWidth, targetHeight);
                croppedCanvas.DrawBitmap(resized, originalRect, targetRect);
            }

            using SKImage image = SKImage.FromBitmap(cropped);
            using MemoryStream outputStream = new MemoryStream();
            image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(outputStream);
            outputStream.Seek(0, SeekOrigin.Begin);
            bitmap.Dispose();

            return new Bitmap(outputStream);
        }

        return bitmap;
    }
}