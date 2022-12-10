using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Toolkit.Foundation
{
    public class TemplateBuilder : ITemplateBuilder
    {
        private readonly List<ITemplateDescriptor> descriptors = new();

        public IReadOnlyCollection<ITemplateDescriptor> Descriptors => new ReadOnlyCollection<ITemplateDescriptor>(descriptors);

        public ITemplateBuilder Add<TViewModel, TView>(string name, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            descriptors.Add(new TemplateDescriptor(typeof(TViewModel), typeof(TView), name, lifetime));
            return this;
        }

        public ITemplateBuilder Add<TViewModel, TView>(ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            descriptors.Add(new TemplateDescriptor(typeof(TViewModel), typeof(TView), null, lifetime));
            return this;
        }
    }
}
