using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Toolkit.UI.Controls.Avalonia;

public class ContentColorPicker : 
    ContentControl
{
    public static readonly StyledProperty<double> PeekOffsetProperty =
           AvaloniaProperty.Register<ContentColorPicker, double>(nameof(PeekOffset), 20);

    public static readonly StyledProperty<int> PeekPixelsProperty =
      AvaloniaProperty.Register<ContentColorPicker, int>(nameof(PeekPixels), 20);

    private readonly Image image = new();

    private Canvas? canvas;
    private (double X, double Y) lastPointerPosition;
    private Border? peekBorder;
    private ZoomBorder? zoomBorder;

    public double PeekOffset
    {
        get => GetValue(PeekOffsetProperty);
        set => SetValue(PeekOffsetProperty, value);
    }

    public int PeekPixels
    {
        get => GetValue(PeekPixelsProperty);
        set => SetValue(PeekPixelsProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs args)
    {
        base.OnApplyTemplate(args);

        PointerMoved -= OnPointerMoved;
        PointerExited -= OnPointerExited;
        PointerEntered -= OnPointerEntered;

        PointerMoved += OnPointerMoved;
        PointerExited += OnPointerExited;
        PointerEntered += OnPointerEntered;

        canvas = args.NameScope.Find<Canvas>("Canvas");

        zoomBorder = args.NameScope.Find<ZoomBorder>("ZoomBorder");
        if (zoomBorder is not null)
        {
            zoomBorder.ZoomChanged += OnZoomChanged;
        }

        peekBorder = args.NameScope.Find<Border>("PeekBorder");
        if (peekBorder is not null)
        {
            peekBorder.Child = image;
        }
    }

    private void OnPointerEntered(object? sender,
        PointerEventArgs args)
    {
        if (peekBorder is not null)
        {
            peekBorder.IsVisible = true;
        }
    }

    private void OnPointerExited(object? sender,
        PointerEventArgs args)
    {
        if (peekBorder is not null)
        {
            peekBorder.IsVisible = false;
        }
    }

    private void OnPointerMoved(object? sender,
        PointerEventArgs args)
    {
        double relativeX = args.GetPosition(canvas).X;
        double relativeY = args.GetPosition(canvas).Y;

        lastPointerPosition = (relativeX, relativeY);

        UpdatePeekPosition(relativeX, relativeY);
    }

    private void OnZoomChanged(object sender,
        ZoomChangedEventArgs args) => UpdatePeekPreview(lastPointerPosition.X, lastPointerPosition.Y);

    private Bitmap RenderToBitmap(Visual visual,
        double centreX,
        double centreY)
    {
        int width = PeekPixels;
        int height = PeekPixels;

        double x = Math.Max(centreX - width / 2, 0);
        double y = Math.Max(centreY - height / 2, 0);

        x = Math.Min(x, visual.Bounds.Width - width);
        y = Math.Min(y, visual.Bounds.Height - height);

        PixelSize pixelSize = new(width, height);
        RenderTargetBitmap renderTarget = new(pixelSize);

        using (DrawingContext drawingContext = renderTarget.CreateDrawingContext())
        {
            drawingContext.PushClip(new Rect(0, 0, width, height));
            drawingContext.FillRectangle(new VisualBrush(visual), new Rect(-x, -y,
                visual.Bounds.Width, visual.Bounds.Height));
        }

        return renderTarget;
    }

    private void UpdatePeekPosition(double relativeX,
        double relativeY)
    {
        if (canvas is null || peekBorder is null)
        {
            return;
        }

        double peekOffset = PeekOffset;

        double newX = relativeX + peekOffset;
        double newY = relativeY + peekOffset;

        newX = Math.Clamp(newX, -peekBorder.Bounds.Width, canvas.Bounds.Width);
        newY = Math.Clamp(newY, -peekBorder.Bounds.Height, canvas.Bounds.Height);

        Canvas.SetLeft(peekBorder, newX);
        Canvas.SetTop(peekBorder, newY);

        bool isPointerInside = relativeX >= -peekOffset && relativeX <= canvas.Bounds.Width + peekOffset &&
            relativeY >= -peekOffset && relativeY <= canvas.Bounds.Height + peekOffset;

        peekBorder.IsVisible = isPointerInside;

        if (isPointerInside)
        {
            UpdatePeekPreview(relativeX, relativeY);
        }
    }

    private void UpdatePeekPreview(double relativeX, 
        double relativeY)
    {
        if (zoomBorder is not null)
        {
            Bitmap bitmap = RenderToBitmap(zoomBorder, relativeX, relativeY);
            image.Source = bitmap;
        }
    }
}