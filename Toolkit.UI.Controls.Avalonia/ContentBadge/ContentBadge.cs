using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Path = Avalonia.Controls.Shapes.Path;

namespace Toolkit.UI.Controls.Avalonia;

public class ContentBadge : 
    ContentControl
{
    public static readonly StyledProperty<string?> BadgePathProperty =
        AvaloniaProperty.Register<ContentBadge, string?>(nameof(BadgePath));

    public static readonly StyledProperty<double> BadgeSizeProperty =
        AvaloniaProperty.Register<ContentBadge, double>(nameof(BadgeSize), 14);

    private ContentControl? badgeContent;

    public string? BadgePath
    {
        get => GetValue(BadgePathProperty);
        set => SetValue(BadgePathProperty, value);
    }

    public double BadgeSize
    {
        get => GetValue(BadgeSizeProperty);
        set => SetValue(BadgeSizeProperty, value);
    }

    public void UpdateClip()
    {
        if (Content is Control content &&
            badgeContent is not null &&
            BadgePath is { Length: > 0 } && 
            Geometry.Parse(BadgePath) is Geometry geometry)
        {
            double backgroundWidth = DesiredSize.Width;
            double backgroundHeight = DesiredSize.Height;

            double scaleX = BadgeSize / geometry.Bounds.Width;
            double scaleY = BadgeSize / geometry.Bounds.Height;

            double adjustedStrokeWidth = Math.Min(scaleX, scaleY) * 8;

            Geometry knockoutGeometry = geometry.GetWidenedGeometry(new Pen(new SolidColorBrush(Colors.Transparent), adjustedStrokeWidth);

            TransformGroup transformGroup = new();
            transformGroup.Children.Add(new ScaleTransform(scaleX, scaleY));

            double scaledWidth = knockoutGeometry.Bounds.Width * scaleX;
            double scaledHeight = knockoutGeometry.Bounds.Height * scaleY;
            double offsetX = backgroundWidth - scaledWidth;
            double offsetY = backgroundHeight - scaledHeight;

            transformGroup.Children.Add(new TranslateTransform(offsetX, offsetY));
            knockoutGeometry.Transform = transformGroup;

            CombinedGeometry combinedGeometry = new()
            {
                GeometryCombineMode = GeometryCombineMode.Exclude,
                Geometry1 = new RectangleGeometry { Rect = new Rect(0, 0, backgroundWidth, backgroundHeight) },
                Geometry2 = knockoutGeometry
            };

            content.Clip = combinedGeometry;

            Geometry overlayGeometry = geometry.Clone();

            TransformGroup overlayTransformGroup = new();
            overlayTransformGroup.Children.Add(new ScaleTransform(scaleX, scaleY));

            overlayTransformGroup.Children.Add(new TranslateTransform(offsetX, offsetY));
            overlayGeometry.Transform = overlayTransformGroup;

            badgeContent.Content = new Path
            {
                Data = overlayGeometry,
                Fill = Foreground
            };
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs args)
    {
        base.OnApplyTemplate(args);
        badgeContent = args.NameScope.Get<ContentControl>("BadgeContent");
    }

    protected override void OnSizeChanged(SizeChangedEventArgs args)
    {
        base.OnSizeChanged(args);
        UpdateClip();
    }
}

