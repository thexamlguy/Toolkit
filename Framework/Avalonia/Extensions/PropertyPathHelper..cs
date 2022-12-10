using Avalonia;
using Avalonia.Data;

namespace Toolkit.Foundation.Avalonia
{
    public static class PropertyPathHelper
    {
        private static readonly Dummy dummy = new();

        public static object GetValue(object args, string path)
        {
            Binding binding = new(path)
            {
                Mode = BindingMode.OneTime,
                Source = args
            };

            dummy.Bind(Dummy.ValueProperty, binding);
            return dummy.GetValue(Dummy.ValueProperty);
        }

        private class Dummy : AvaloniaObject
        {
            public static readonly StyledProperty<object> ValueProperty =
                AvaloniaProperty.Register<Dummy, object>("Value");
        }
    }
}
