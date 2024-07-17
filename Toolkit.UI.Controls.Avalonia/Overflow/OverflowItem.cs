using Avalonia;
using Avalonia.Controls;

namespace Toolkit.UI.Controls.Avalonia;

public class OverflowItem : 
    ListBoxItem
{
    public static readonly StyledProperty<double> BadgeSizeProperty =
        AvaloniaProperty.Register<OverflowItem, double>(nameof(BadgeSize), 14);

    public static readonly StyledProperty<bool> IsBadgeVisibleProperty =
        AvaloniaProperty.Register<OverflowItem, bool>(nameof(IsBadgeVisible), true);

    public static readonly StyledProperty<string> BadgePathProperty =
        AvaloniaProperty.Register<OverflowItem, string>(nameof(BadgePath));

    public static readonly StyledProperty<ContentBadgePlacement> BadgePlacementProperty =
         AvaloniaProperty.Register<OverflowItem, ContentBadgePlacement>(nameof(BadgePlacement), ContentBadgePlacement.BottomRight);

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
}

