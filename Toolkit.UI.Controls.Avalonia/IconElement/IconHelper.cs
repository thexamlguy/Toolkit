using System.Reflection;

namespace Toolkit.UI.Controls.Avalonia;

public class IconHelper
{
    private static MethodInfo? invoker;

    public static FluentAvalonia.UI.Controls.FAIconElement? CreateIconElement(FluentAvalonia.UI.Controls.IconSource source)
    {
        if (source is ContentIconSource contentIconSource)
        {
            ContentIcon contentIcon = new()
            {
                [!ContentIcon.ContentProperty] = contentIconSource[!ContentIconSource.ContentProperty],
                [!ContentIcon.ContentTemplateProperty] = contentIconSource[!ContentIconSource.ContentTemplateProperty],
            };

            return contentIcon;
        }
        else
        {
            if (invoker == null)
            {
                Type? iconHelpersType = Type.GetType("FluentAvalonia.UI.Controls.IconHelpers,FluentAvalonia");
                if (iconHelpersType?.GetMethod("CreateFromUnknown", BindingFlags.Public | BindingFlags.Static) is MethodInfo createFromUnknown)
                {
                    invoker = createFromUnknown;
                }
            }

            return (FluentAvalonia.UI.Controls.FAIconElement?)invoker?.Invoke(null, new object[] { source });
        }
    }
}