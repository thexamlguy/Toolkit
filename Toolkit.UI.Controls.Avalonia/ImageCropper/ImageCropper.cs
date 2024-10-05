using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Toolkit.UI.Controls.Avalonia;

public class ImageCropper : TemplatedControl
{
    public static readonly StyledProperty<IImage?> CurrentAreaBitmapProperty =
        AvaloniaProperty.Register<ImageCropper, IImage?>(nameof(CurrentAreaBitmap));

    public static readonly StyledProperty<Rect> CurrentRectProperty =
        AvaloniaProperty.Register<ImageCropper, Rect>(nameof(CurrentRect));

    public static readonly StyledProperty<bool> IsRatioScaleProperty =
        AvaloniaProperty.Register<ImageCropper, bool>(nameof(IsRatioScale));

    public static readonly StyledProperty<double> RectScaleProperty =
        AvaloniaProperty.Register<ImageCropper, double>(nameof(RectScale), 0.5);

    public static readonly StyledProperty<Size> ScaleSizeProperty =
        AvaloniaProperty.Register<ImageCropper, Size>(nameof(ScaleSize), new Size(2, 1));

    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<ImageCropper, IImage?>(nameof(Source));

    private Border? border;
    private Thumb? bottomLeftButton;
    private Thumb? bottomRightButton;
    private Canvas? canvas;
    private bool isDragging;
    private double offsetX;
    private double offsetY;
    private Rectangle? rectangleBottom;
    private Rectangle? rectangleLeft;
    private Rectangle? rectangleRight;
    private Rectangle? rectangleTop;

    private Thumb? topLeftButton;
    private Thumb? topRightButton;

    static ImageCropper()
    {
        AffectsRender<ImageCropper>(SourceProperty, RectScaleProperty);
    }

    public IImage? CurrentAreaBitmap
    {
        get => GetValue(CurrentAreaBitmapProperty);
        private set => SetValue(CurrentAreaBitmapProperty, value);
    }

    public Rect CurrentRect
    {
        get => GetValue(CurrentRectProperty);
        private set => SetValue(CurrentRectProperty, value);
    }

    public bool IsRatioScale
    {
        get => GetValue(IsRatioScaleProperty);
        set => SetValue(IsRatioScaleProperty, value);
    }

    public double RectScale
    {
        get => GetValue(RectScaleProperty);
        set => SetValue(RectScaleProperty, value);
    }

    public Size ScaleSize
    {
        get => GetValue(ScaleSizeProperty);
        set => SetValue(ScaleSizeProperty, value);
    }

    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs args)
    {
        base.OnApplyTemplate(args);

        canvas = args.NameScope.Find<Canvas>("Canvas");
        rectangleLeft = args.NameScope.Find<Rectangle>("RectangleLeft");
        rectangleTop = args.NameScope.Find<Rectangle>("RectangleTop");
        rectangleRight = args.NameScope.Find<Rectangle>("RectangleRight");
        rectangleBottom = args.NameScope.Find<Rectangle>("RectangleBottom");
        border = args.NameScope.Find<Border>("Border");

        topLeftButton = args.NameScope.Find<Thumb>("TopLeftButton");
        if (topLeftButton is not null)
        {
            topLeftButton.DragDelta += OnThumbDragDelta;
        }

        topRightButton = args.NameScope.Find<Thumb>("TopRightButton");
        if (topRightButton is not null)
        {
            topRightButton.DragDelta += OnThumbDragDelta;
        }

        bottomLeftButton = args.NameScope.Find<Thumb>("BottomLeftButton");
        if (bottomLeftButton is not null)
        {
            bottomLeftButton.DragDelta += OnThumbDragDelta;
        }

        bottomRightButton = args.NameScope.Find<Thumb>("BottomRightButton");
        if (bottomRightButton is not null)
        {
            bottomRightButton.DragDelta += OnThumbDragDelta;
        }

        DrawImage();
    }

    protected override void OnLoaded(RoutedEventArgs args)
    {
        base.OnLoaded(args);
        DrawImage();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SourceProperty ||
            change.Property == IsRatioScaleProperty ||
            change.Property == RectScaleProperty)
        {
            DrawImage();
        }
    }

    private void DrawImage()
    {
        if (canvas is null || Source is not Bitmap bitmap)
        {
            return;
        }

        double maxWidth = DesiredSize.Width;
        double maxHeight = DesiredSize.Height;

        double imageWidth = Source.Size.Width;
        double imageHeight = Source.Size.Height;

        double scaleFactor = Math.Min(maxWidth / imageWidth, maxHeight / imageHeight);
        double width = imageWidth * scaleFactor;
        double height = imageHeight * scaleFactor;

        canvas.Width = width;
        canvas.Height = height;

        canvas.Background = new ImageBrush
        {
            Source = bitmap
        };

        UpdatePunchThrough(width, height);
        Render();
    }

    private void OnBorderPointerMoved(object? sender,
        PointerEventArgs args)
    {
        if (!isDragging || canvas is null || border is null)
        {
            return;
        }

        Point position = args.GetPosition(this);
        double newX = Math.Clamp(position.X - offsetX, 0, canvas.Bounds.Width - border.Bounds.Width);
        double newY = Math.Clamp(position.Y - offsetY, 0, canvas.Bounds.Height - border.Bounds.Height);

        Canvas.SetLeft(border, newX);
        Canvas.SetTop(border, newY);

        PositionThumbs();
        Render();
    }

    private void OnBorderPointerPressed(object? sender,
        PointerPressedEventArgs args)
    {
        if (!isDragging && border is not null)
        {
            isDragging = true;
            Point position = args.GetPosition(this);

            offsetX = position.X - Canvas.GetLeft(border);
            offsetY = position.Y - Canvas.GetTop(border);
        }
    }

    private void OnBorderPointerReleased(object? sender,
        PointerReleasedEventArgs args) => isDragging = false;

    private void OnThumbDragDelta(object? sender, VectorEventArgs args)
    {
        if (canvas is null || border is null || sender is not Thumb thumb)
        {
            return;
        }

        double deltaX = args.Vector.X;
        double deltaY = args.Vector.Y;

        double leftPosition = Canvas.GetLeft(border);
        double topPosition = Canvas.GetTop(border);
        double newWidth = border.Width;
        double newHeight = border.Height;

        switch (thumb.Name)
        {
            case "TopLeftButton":
                newWidth = Math.Max(0, border.Width - deltaX);
                newHeight = Math.Max(0, border.Height - deltaY);
                leftPosition += deltaX;
                topPosition += deltaY;
                break;

            case "TopRightButton":
                newWidth = Math.Max(0, border.Width + deltaX);
                newHeight = Math.Max(0, border.Height - deltaY);
                topPosition += deltaY;
                break;

            case "BottomLeftButton":
                newWidth = Math.Max(0, border.Width - deltaX);
                newHeight = Math.Max(0, border.Height + deltaY);
                leftPosition += deltaX;
                break;

            case "BottomRightButton":
                newWidth = Math.Max(0, border.Width + deltaX);
                newHeight = Math.Max(0, border.Height + deltaY);
                break;
        }

        if (newWidth < 0 || newHeight < 0)
        {
            return;
        }

        if (thumb.Name == "TopLeftButton" || thumb.Name == "BottomLeftButton")
        {
            leftPosition = Math.Max(0, leftPosition);
            newWidth = Math.Max(0, border.Width - (leftPosition - Canvas.GetLeft(border)));
        }
        else if (thumb.Name == "TopRightButton" || thumb.Name == "BottomRightButton")
        {
            double rightBoundary = canvas.Width;
            newWidth = Math.Min(newWidth, rightBoundary - leftPosition);
        }

        if (thumb.Name == "TopLeftButton" || thumb.Name == "TopRightButton")
        {
            topPosition = Math.Max(0, topPosition);
            newHeight = Math.Max(0, border.Height - (topPosition - Canvas.GetTop(border)));
        }
        else if (thumb.Name == "BottomLeftButton" || thumb.Name == "BottomRightButton")
        {
            double bottomBoundary = canvas.Height;
            newHeight = Math.Min(newHeight, bottomBoundary - topPosition);
        }

        border.Width = newWidth;
        border.Height = newHeight;

        Canvas.SetLeft(border, leftPosition);
        Canvas.SetTop(border, topPosition);

        PositionThumbs();
        Render();
    }


    private void PositionThumbs()
    {
        if (border == null ||
            canvas == null)
        {
            return;
        }

        double borderLeft = Canvas.GetLeft(border);
        double borderTop = Canvas.GetTop(border);
        double borderWidth = border.Width;
        double borderHeight = border.Height;

        if (topLeftButton is not null)
        {
            Canvas.SetLeft(topLeftButton, borderLeft - (topLeftButton.Width / 2));
            Canvas.SetTop(topLeftButton, borderTop - (topLeftButton.Height / 2));
        }

        if (topRightButton is not null)
        {
            Canvas.SetLeft(topRightButton, borderLeft + borderWidth - (topRightButton.Width / 2));
            Canvas.SetTop(topRightButton, borderTop - (topRightButton.Height / 2));
        }

        if (bottomLeftButton is not null)
        {
            Canvas.SetLeft(bottomLeftButton, borderLeft - (bottomLeftButton.Width / 2));
            Canvas.SetTop(bottomLeftButton, borderTop + borderHeight - (bottomLeftButton.Height / 2));
        }

        if (bottomRightButton is not null)
        {
            Canvas.SetLeft(bottomRightButton, borderLeft + borderWidth - (bottomRightButton.Width / 2));
            Canvas.SetTop(bottomRightButton, borderTop + borderHeight - (bottomRightButton.Height / 2));
        }
    }

    private void Render()
    {
        if (canvas == null ||
            border == null ||
            rectangleLeft == null ||
            rectangleTop == null ||
            rectangleRight == null ||
            rectangleBottom == null)
        {
            return;
        }

        double borderTop = Canvas.GetTop(border);
        double borderLeft = Canvas.GetLeft(border);

        rectangleLeft.Width = Math.Max(0, borderLeft);
        rectangleLeft.Height = Math.Max(0, border.Height);
        Canvas.SetTop(rectangleLeft, borderTop);

        rectangleTop.Width = Math.Max(0, canvas.Width);
        rectangleTop.Height = Math.Max(0, borderTop);

        double rightX = borderLeft + border.Width;

        rectangleRight.Width = Math.Max(0, canvas.Width - rightX);
        rectangleRight.Height = Math.Max(0, border.Height);
        Canvas.SetLeft(rectangleRight, rightX);
        Canvas.SetTop(rectangleRight, borderTop);

        double bottomY = borderTop + border.Height;

        rectangleBottom.Width = Math.Max(0, canvas.Width);
        rectangleBottom.Height = Math.Max(0, canvas.Height - bottomY);
        Canvas.SetTop(rectangleBottom, bottomY);
    }

    private void UpdatePunchThrough(double width,
        double height)
    {
        if (canvas == null || 
            border == null)
        {
            return;
        }

        if (IsRatioScale && ScaleSize.Width > 0 && ScaleSize.Height > 0)
        {
            if (ScaleSize.Width > ScaleSize.Height)
            {
                border.Width = width * RectScale;
                border.Height = border.Width / ScaleSize.Width;
            }
            else
            {
                border.Height = height * RectScale;
                border.Width = border.Height * ScaleSize.Height;
            }
        }
        else
        {
            border.Width = width * RectScale;
            border.Height = height * RectScale;
        }

        double centreX = (canvas.Width - border.Width) / 2;
        double centreY = (canvas.Height - border.Height) / 2;

        Canvas.SetLeft(border, centreX);
        Canvas.SetTop(border, centreY);

        PositionThumbs();

        border.PointerPressed -= OnBorderPointerPressed;
        border.PointerPressed += OnBorderPointerPressed;
        border.PointerMoved -= OnBorderPointerMoved;
        border.PointerMoved += OnBorderPointerMoved;
        border.PointerReleased -= OnBorderPointerReleased;
        border.PointerReleased += OnBorderPointerReleased;
    }
}

