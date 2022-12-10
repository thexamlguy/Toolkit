using Avalonia.Data;

namespace Toolkit.Foundation.Avalonia
{
    public static class MarkupExtensions
    {
        public static Binding? ToBinding(this object value)
        {
            if (value is Binding)
            {
                return value as Binding;
            }

            return new Binding { Mode = BindingMode.OneWay, Source = value };
        }
    }
}