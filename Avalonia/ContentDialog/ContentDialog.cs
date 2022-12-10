using Avalonia.Styling;

namespace Toolkit.Controls.Avalonia;

public class ContentDialog : FluentAvalonia.UI.Controls.ContentDialog, IStyleable
{
    Type IStyleable.StyleKey => typeof(FluentAvalonia.UI.Controls.ContentDialog);
}