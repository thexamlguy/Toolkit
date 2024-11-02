using Avalonia.Controls;

namespace Toolkit.UI.Controls.Avalonia;

public class ListViewItem : 
    ListBoxItem
{
    protected override Type StyleKeyOverride =>
          typeof(ListBoxItem);
}
