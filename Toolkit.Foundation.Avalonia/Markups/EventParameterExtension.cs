using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Toolkit.Foundation.Avalonia
{
    public class EventParameterExtension : MarkupExtension, IEventParameter
    {
        private readonly IValueConverter? converter;

        private readonly object? converterParameter;

        private readonly string? key;
        private readonly string? path;

        public EventParameterExtension()
        {

        }

        public EventParameterExtension(string key, string path)
        {
            this.key = key;
            this.path = path;
        }

        public EventParameterExtension(string path)
        {
            this.path = path;
        }

        public EventParameterExtension(IValueConverter? converter = null, object? converterParameter = null)
        {
            this.converter = converter;
            this.converterParameter = converterParameter;
        }

        public List<object> GetValues(EventArgs args)
        {
            List<object>? parameters = new();

            dynamic? arguments = args.GetEventArguments(path, converter, converterParameter);
            if (arguments is not null)
            {
                if (arguments is ICollection<object> collection)
                {
                    foreach (object? argument in collection)
                    {
                        parameters.Add(key is not null ? new KeyValuePair<string, object>(key, (dynamic)argument) : argument);
                    }
                }
                else
                {
                    parameters.Add(key is not null ? new KeyValuePair<string, object>(key, arguments) : arguments);
                }
            }

            return parameters;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}