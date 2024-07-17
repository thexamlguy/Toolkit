using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Path = Avalonia.Controls.Shapes.Path;

namespace Toolkit.UI.Controls.Avalonia;
public class ContentBadge : 
    ContentControl
{
    public static readonly StyledProperty<string> BadgePathProperty =
        AvaloniaProperty.Register<ContentBadge, string>(nameof(BadgePath));

    public static readonly StyledProperty<ContentBadgePlacement> BadgePlacementProperty =
         AvaloniaProperty.Register<ContentBadge, ContentBadgePlacement>(nameof(BadgePlacement), ContentBadgePlacement.BottomRight);

    public static readonly StyledProperty<double> BadgeSizeProperty =
        AvaloniaProperty.Register<ContentBadge, double>(nameof(BadgeSize), 14);

    public static readonly StyledProperty<bool> IsBadgeVisibleProperty =
        AvaloniaProperty.Register<ContentBadge, bool>(nameof(IsBadgeVisible), true);

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == BadgePathProperty ||
            change.Property == BadgeSizeProperty ||
            change.Property == IsBadgeVisibleProperty)
        {
            UpdateBadge();
        }
    }

    private ContentControl? badgeContent;

    public string BadgePath
    {
        get => GetValue(BadgePathProperty);
        set => SetValue(BadgePathProperty, value);
    }

    public ContentBadgePlacement BadgePlacement
    {
        get => GetValue(BadgePlacementProperty);
        set => SetValue(BadgePlacementProperty, value);
    }

    public double BadgeSize
    {
        get => GetValue(BadgeSizeProperty);
        set => SetValue(BadgeSizeProperty, value);
    }

    public bool IsBadgeVisible
    {
        get => GetValue(IsBadgeVisibleProperty);
        set => SetValue(IsBadgeVisibleProperty, value);
    }

    public void UpdateBadge()
    {
        if (Content is Control content && badgeContent is not null)
        {
            if (IsBadgeVisible &&
                BadgePath is { Length: > 0 } &&
                Geometry.Parse(BadgePath) is Geometry geometry)
            {
                double backgroundWidth = DesiredSize.Width;
                double backgroundHeight = DesiredSize.Height;

                double badgeWidth = geometry.Bounds.Width;
                double badgeHeight = geometry.Bounds.Height;
                double scale = BadgeSize / Math.Max(badgeWidth, badgeHeight);

                double scaleX = scale;
                double scaleY = scale;

                double adjustedStrokeWidth = Math.Min(scaleX, scaleY) * 8;

                Geometry knockoutGeometry = geometry.GetWidenedGeometry(new Pen(new SolidColorBrush(Colors.Transparent), adjustedStrokeWidth));

                TransformGroup transformGroup = new();
                transformGroup.Children.Add(new ScaleTransform(scaleX, scaleY));

                double scaledWidth = knockoutGeometry.Bounds.Width * scaleX;
                double scaledHeight = knockoutGeometry.Bounds.Height * scaleY;

                double offsetX = 0;
                double offsetY = 0;

                switch (BadgePlacement)
                {
                    case ContentBadgePlacement.TopLeft:
                        offsetX = 0;
                        offsetY = 0;
                        break;
                    case ContentBadgePlacement.TopRight:
                        offsetX = backgroundWidth - scaledWidth;
                        offsetY = 0;
                        break;
                    case ContentBadgePlacement.BottomLeft:
                        offsetX = 0;
                        offsetY = backgroundHeight - scaledHeight;
                        break;
                    case ContentBadgePlacement.BottomRight:
                        offsetX = backgroundWidth - scaledWidth;
                        offsetY = backgroundHeight - scaledHeight;
                        break;
                }

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
            else
            {
                badgeContent.Content = null;
                content.Clip = null;
            }
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
        UpdateBadge();
    }
}

