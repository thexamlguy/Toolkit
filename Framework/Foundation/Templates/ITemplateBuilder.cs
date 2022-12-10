using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation
{
    public interface ITemplateBuilder
    {
        IReadOnlyCollection<ITemplateDescriptor> Descriptors { get; }

        ITemplateBuilder Add<TViewModel, TView>(string name, ServiceLifetime lifetime = ServiceLifetime.Transient);

        ITemplateBuilder Add<TViewModel, TView>(ServiceLifetime lifetime = ServiceLifetime.Transient);
    }
}
