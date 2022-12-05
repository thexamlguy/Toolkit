namespace Toolkit.Foundation
{
    public class Navigated<TContent, TDataContext> where TContent : class where TDataContext : class
    {
        public Navigated()
        {
        }

        public Navigated(TContent content, TDataContext dataContext, IDictionary<string, object>? parameters = null)
        {
            Content = content;
            DataContext = dataContext;
            Parameters = parameters;
        }

        public TContent? Content { get; }

        public TDataContext? DataContext { get; }

        public IDictionary<string, object>? Parameters { get; }
    }

    public class Navigated
    {
        public static Navigated<TTemplate, TDataTemplate> Create<TTemplate, TDataTemplate>(TTemplate content, TDataTemplate dataContext, IDictionary<string, object>? parameters = null) where TTemplate : class where TDataTemplate : class
        {
            return new Navigated<TTemplate, TDataTemplate>(content, dataContext, parameters);
        }
    }
}