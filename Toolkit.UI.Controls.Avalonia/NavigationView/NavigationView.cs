using FluentAvalonia.UI.Controls;
using System.Reflection;

namespace Toolkit.UI.Controls.Avalonia;

public class NavigationView :
    FluentAvalonia.UI.Controls.NavigationView
{
    public NavigationView()
    {
        ItemInvoked += OnItemInvoked;
    }

    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.NavigationView);

    private void ApplyNavigationFix()
    {
        if (typeof(FluentAvalonia.UI.Controls.NavigationView)
            .GetField("_shouldRaiseItemInvokedAfterSelection",
                BindingFlags.NonPublic | BindingFlags.Instance)
                is FieldInfo shouldRaiseItemInvokedAfterSelectionFieldInfo)
        {
            if (shouldRaiseItemInvokedAfterSelectionFieldInfo.GetValue(this) is bool value)
            {
                if (value)
                {
                    shouldRaiseItemInvokedAfterSelectionFieldInfo.SetValue(this, false);
                }
            }
        };
    }

    private void OnItemInvoked(object? sender,
        NavigationViewItemInvokedEventArgs args)
    {
        ApplyNavigationFix();
    }
}