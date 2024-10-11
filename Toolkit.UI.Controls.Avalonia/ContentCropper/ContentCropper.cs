using Avalonia.Controls.Primitives;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia;
using Avalonia.Media;
using Path = Avalonia.Controls.Shapes.Path;

namespace Toolkit.UI.Controls.Avalonia;

public class ContentCropper : ContentControl
{
    public static readonly StyledProperty<Rect> CropRectangleProperty =
        AvaloniaProperty.Register<ContentCropper, Rect>(nameof(CropRectangle));

    public static readonly StyledProperty<bool> IsRatioScaleProperty =
        AvaloniaProperty.Register<ContentCropper, bool>(nameof(IsRatioScale));

    public static readonly StyledProperty<double> RectScaleProperty =
        AvaloniaProperty.Register<ContentCropper, double>(nameof(RectScale), 0.5);

    public static readonly StyledProperty<Size> ScaleSizeProperty =
        AvaloniaProperty.Register<ContentCropper, Size>(nameof(ScaleSize), new Size(2, 1));

    private Border? border;
    private Thumb? bottomLeftButton;
    private Thumb? bottomRightButton;
    private Canvas? canvas;
    private double cropHeightRatio;
    private double cropLeftRatio;
    private double cropTopRatio;
    private double cropWidthRatio;
    private bool isDragging;
    private double offsetX;
    private double offsetY;
    private Path? overlayPath;
    private Thumb? topLeftButton;
    private Thumb? topRightButton;
    private Thumb? leftButton;
    private Thumb? rightButton;
    private Thumb? topButton;
    private Thumb? bottomButton;

    static ContentCropper()
    {
        AffectsRender<ContentCropper>(RectScaleProperty, ContentProperty);
    }

    public Rect CropRectangle
    {
        get => GetValue(CropRectangleProperty);
        private set => SetValue(CropRectangleProperty, value);
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs args)
    {
        base.OnApplyTemplate(args);

        canvas = args.NameScope.Find<Canvas>("Canvas");
        overlayPath = args.NameScope.Find<Path>("OverlayPath");
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

        leftButton = args.NameScope.Find<Thumb>("LeftButton");
        if (leftButton is not null)
        {
            leftButton.DragDelta += OnThumbDragDelta;
        }

        rightButton = args.NameScope.Find<Thumb>("RightButton");
        if (rightButton is not null)
        {
            rightButton.DragDelta += OnThumbDragDelta;
        }

        topButton = args.NameScope.Find<Thumb>("TopButton");
        if (topButton is not null)
        {
            topButton.DragDelta += OnThumbDragDelta;
        }

        bottomButton = args.NameScope.Find<Thumb>("BottomButton");
        if (bottomButton is not null)
        {
            bottomButton.DragDelta += OnThumbDragDelta;
        }

    }

    protected override void OnLoaded(RoutedEventArgs args)
    {
        base.OnLoaded(args);
        InitializeCropRect();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsRatioScaleProperty ||
            change.Property == RectScaleProperty || 
            change.Property == ContentProperty)
        {
            InitializeCropRect();
        }
    }

    protected override void OnSizeChanged(SizeChangedEventArgs args)
    {
        base.OnSizeChanged(args);

        if (canvas is null || border is null)
        {
            return;
        }

        double newContentWidth = Bounds.Width;
        double newContentHeight = Bounds.Height;

        canvas.Width = newContentWidth;
        canvas.Height = newContentHeight;

        if (border.Width > 0 && border.Height > 0)
        {
            double newCropLeft = cropLeftRatio * newContentWidth;
            double newCropTop = cropTopRatio * newContentHeight;
            double newCropWidth = cropWidthRatio * newContentWidth;
            double newCropHeight = cropHeightRatio * newContentHeight;

            border.Width = newCropWidth;
            border.Height = newCropHeight;

            Canvas.SetLeft(border, newCropLeft);
            Canvas.SetTop(border, newCropTop);
        }
        else
        {
            InitializeCropRect();
        }

        UpdateCropRectangle();

        PositionThumbs();
        RenderOverLays();
    }

    private void InitializeCropRect()
    {
        if (canvas is null || Content is not Control content)
        {
            return;
        }

        double maxWidth = Bounds.Width;
        double maxHeight = Bounds.Height;

        double contentWidth = content.Bounds.Width > 0 ? content.Bounds.Width : maxWidth * 0.5;
        double contentHeight = content.Bounds.Height > 0 ? content.Bounds.Height : maxHeight * 0.5;

        double scaleFactor = Math.Min(maxWidth / contentWidth, maxHeight / contentHeight);
        double width = contentWidth * scaleFactor;
        double height = contentHeight * scaleFactor;

        canvas.Width = width;
        canvas.Height = height;

        UpdateCropArea(width, height);
        UpdateCropRatios();
        UpdateCropRectangle();

        PositionThumbs();
        RenderOverLays();
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

        UpdateCropRectangle();

        PositionThumbs();
        RenderOverLays();
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
        PointerReleasedEventArgs args)
    {
        isDragging = false;
        UpdateCropRatios();
    }

    private void OnThumbDragDelta(object? sender, VectorEventArgs args)
    {
        if (canvas is null || border is null || sender is not Thumb thumb)
        {
            return;
        }
        
        double minimumWidth = 20; 
        double minimumHeight = 20; 

        double deltaX = args.Vector.X;
        double deltaY = args.Vector.Y;

        double leftPosition = Canvas.GetLeft(border);
        double topPosition = Canvas.GetTop(border);
        double newWidth = border.Width;
        double newHeight = border.Height;

        bool canResizeWidth = true;
        bool canResizeHeight = true;

        switch (thumb.Name)
        {
            case "TopLeftButton":
                if (border.Width - deltaX < minimumWidth)
                {
                    canResizeWidth = false;
                }

                if (border.Height - deltaY < minimumHeight)
                {
                    canResizeHeight = false;
                }

                if (canResizeWidth)
                {
                    newWidth = border.Width - deltaX;
                    leftPosition += deltaX;
                }

                if (canResizeHeight)
                {
                    newHeight = border.Height - deltaY;
                    topPosition += deltaY;
                }
                break;

            case "TopRightButton":
                if (border.Height - deltaY < minimumHeight)
                {
                    canResizeHeight = false;
                }

                newWidth = border.Width + deltaX;
                if (canResizeHeight)
                {
                    newHeight = border.Height - deltaY;
                    topPosition += deltaY;
                }
                break;

            case "BottomLeftButton":
                if (border.Width - deltaX < minimumWidth)
                {
                    canResizeWidth = false;
                }

                newWidth = border.Width - deltaX;
                if (canResizeWidth)
                {
                    leftPosition += deltaX;
                }

                newHeight = border.Height + deltaY;
                break;

            case "BottomRightButton":
                newWidth = border.Width + deltaX;
                newHeight = border.Height + deltaY;
                break;

            case "TopButton":
                if (border.Height - deltaY < minimumHeight)
                {
                    canResizeHeight = false;
                }

                if (canResizeHeight)
                {
                    newHeight = border.Height - deltaY;
                    topPosition += deltaY;
                }
                break;

            case "BottomButton":
                newHeight = border.Height + deltaY;
                break;

            case "LeftButton":
                if (border.Width - deltaX < minimumWidth)
                {
                    canResizeWidth = false;
                }

                if (canResizeWidth)
                {
                    newWidth = border.Width - deltaX;
                    leftPosition += deltaX;
                }
                break;

            case "RightButton":
                newWidth = border.Width + deltaX;
                break;
        }

        if (leftPosition < 0 || leftPosition + newWidth > canvas.Width ||
            topPosition < 0 || topPosition + newHeight > canvas.Height ||
            newWidth < minimumWidth || newHeight < minimumHeight)
        {
            return;
        }

        border.Width = newWidth;
        border.Height = newHeight;

        Canvas.SetLeft(border, leftPosition);
        Canvas.SetTop(border, topPosition);

        UpdateCropRatios();
        UpdateCropRectangle();

        PositionThumbs();
        RenderOverLays();
    }

    private void PositionThumbs()
    {
        if (border == null || canvas == null) return;

        double borderLeft = Canvas.GetLeft(border);
        double borderTop = Canvas.GetTop(border);
        double borderWidth = border.Width;
        double borderHeight = border.Height;

        if (topLeftButton is not null)
        {
            Canvas.SetLeft(topLeftButton, borderLeft);
            Canvas.SetTop(topLeftButton, borderTop);
        }

        if (topRightButton is not null)
        {
            Canvas.SetLeft(topRightButton, borderLeft + borderWidth - topRightButton.Width);
            Canvas.SetTop(topRightButton, borderTop);
        }

        if (bottomLeftButton is not null)
        {
            Canvas.SetLeft(bottomLeftButton, borderLeft);
            Canvas.SetTop(bottomLeftButton, borderTop + borderHeight - bottomLeftButton.Height);
        }

        if (bottomRightButton is not null)
        {
            Canvas.SetLeft(bottomRightButton, borderLeft + borderWidth - bottomRightButton.Width);
            Canvas.SetTop(bottomRightButton, borderTop + borderHeight - bottomRightButton.Height);
        }

        if (leftButton is not null)
        {
            Canvas.SetLeft(leftButton, borderLeft);
            Canvas.SetTop(leftButton, borderTop + borderHeight / 2 - leftButton.Height / 2);
        }

        if (rightButton is not null)
        {
            Canvas.SetLeft(rightButton, borderLeft + borderWidth - rightButton.Width);
            Canvas.SetTop(rightButton, borderTop + borderHeight / 2 - rightButton.Height / 2);
        }

        if (topButton is not null)
        {
            Canvas.SetLeft(topButton, borderLeft + borderWidth / 2 - topButton.Width / 2);
            Canvas.SetTop(topButton, borderTop);
        }

        if (bottomButton is not null)
        {
            Canvas.SetLeft(bottomButton, borderLeft + borderWidth / 2 - bottomButton.Width / 2);
            Canvas.SetTop(bottomButton, borderTop + borderHeight - bottomButton.Height);
        }
    }

    private void RenderOverLays()
    {
        if (canvas == null || border == null || overlayPath == null)
        {
            return;
        }

        double borderTop = Canvas.GetTop(border);
        double borderLeft = Canvas.GetLeft(border);
        double borderWidth = border.Width;
        double borderHeight = border.Height;

        RectangleGeometry outerRect = new(new Rect(0, 0, 
            canvas.Width, 
            canvas.Height));

        RectangleGeometry innerRect = new(new Rect(borderLeft, 
            borderTop, 
            borderWidth, 
            borderHeight));

        CombinedGeometry punchThroughGeometry = new(GeometryCombineMode.Exclude, 
            outerRect,
            innerRect);

        overlayPath.Data = punchThroughGeometry;
    }

    private void UpdateCropArea(double width, double height)
    {
        if (canvas == null || border == null)
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

    private void UpdateCropRatios()
    {
        if (canvas == null || border == null)
        {
            return;
        }

        cropLeftRatio = Canvas.GetLeft(border) / canvas.Width;
        cropTopRatio = Canvas.GetTop(border) / canvas.Height;
        cropWidthRatio = border.Width / canvas.Width;
        cropHeightRatio = border.Height / canvas.Height;
    }

    private void UpdateCropRectangle()
    {
        if (canvas is null || border is null)
        {
            return;
        }

        double left = Canvas.GetLeft(border);
        double top = Canvas.GetTop(border);

        CropRectangle = new Rect(left, top, border.Width, border.Height);
    }
}
