using Avalonia.Controls;
using Avalonia;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace Toolkit.Foundation.Avalonia
{
    public class ParameterBindingExtension : MarkupExtension, IParameter
    {
        private static readonly AttachedProperty<object> ValueProperty =
            AvaloniaProperty.RegisterAttached<ParameterBindingExtension, Control, object>("Value");

        private readonly Binding? valueBinding;

        public ParameterBindingExtension(string key, object value)
        {
            Key = key;
            valueBinding = value.ToBinding();
        }

        public string? Key { get; }

        public KeyValuePair<string, object>? GetValue(object target)
        {
            if (target is AvaloniaObject avaloniaObject)
            {
                if (valueBinding is not null)
                {
                    avaloniaObject.Bind(ValueProperty, valueBinding);
                    return new KeyValuePair<string, object>(Key, (dynamic)avaloniaObject.GetValue(ValueProperty));
                }
            }

            return default;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
