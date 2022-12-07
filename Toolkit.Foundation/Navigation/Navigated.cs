namespace Toolkit.Foundation
{
    public class Navigated<TTemplate, TContent> where TTemplate : class where TContent : class
    {
        public Navigated()
        {
        }

        public Navigated(TTemplate template, TContent content, IDictionary<string, object>? parameters = null)
        {
            Template = template;
            Content = content;
            Parameters = parameters;
        }

        public TTemplate? Template { get; }

        public TContent? Content { get; }

        public IDictionary<string, object>? Parameters { get; }
    }

    public class Navigated
    {
        public static Navigated<TTemplate, TContent> Create<TTemplate, TContent>(TTemplate template, TContent? content, IDictionary<string, object>? parameters = null) where TTemplate : class where TContent : class
        {
            return new Navigated<TTemplate, TContent>(template, content, parameters);
        }
    }
}