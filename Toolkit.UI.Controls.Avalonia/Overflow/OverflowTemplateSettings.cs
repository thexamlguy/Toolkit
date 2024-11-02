using Avalonia;

namespace Toolkit.UI.Controls.Avalonia;

public class OverflowTemplateSettings :
    AvaloniaObject
{
    public static readonly StyledProperty<object?> PrimarySelectionProperty =
        AvaloniaProperty.Register<OverflowTemplateSettings, object?>(nameof(PrimarySelection));

    public static readonly StyledProperty<object?> SecondarySelectionProperty =
        AvaloniaProperty.Register<OverflowTemplateSettings, object?>(nameof(SecondarySelection));

    private bool isSelectionChanging;

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        base.OnPropertyChanged(args);

        if (!isSelectionChanging)
        {
            isSelectionChanging = true;

            if (args.Property == PrimarySelectionProperty)
            {
                SecondarySelection = null;
            }
            else if (args.Property == SecondarySelectionProperty)
            {
                PrimarySelection = null;
            }

            isSelectionChanging = false;
        }
    }

    public object? PrimarySelection
    {
        get => GetValue(PrimarySelectionProperty);
        set => SetValue(PrimarySelectionProperty, value);
    }

    public object? SecondarySelection
    {
        get => GetValue(SecondarySelectionProperty);
        set => SetValue(SecondarySelectionProperty, value);
    }
}