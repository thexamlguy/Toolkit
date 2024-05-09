using Avalonia;
using Avalonia.Media;

namespace Toolkit.UI.Controls.Avalonia;

public class PersonPictureTemplateSettings : AvaloniaObject
{
    private static readonly StyledProperty<ImageBrush?> ActualImageBrushProperty =
        AvaloniaProperty.Register<PersonPictureTemplateSettings, ImageBrush?>(nameof(ActualImageBrush));


    private static readonly StyledProperty<string> ActualInitialsProperty =
        AvaloniaProperty.Register<PersonPictureTemplateSettings, string>(nameof(ActualInitials));

    public ImageBrush? ActualImageBrush
    {
        get => GetValue(ActualImageBrushProperty);
        set => SetValue(ActualImageBrushProperty, value);
    }

    public string ActualInitials
    {
        get => GetValue(ActualInitialsProperty);
        set => SetValue(ActualInitialsProperty, value);
    }
}
