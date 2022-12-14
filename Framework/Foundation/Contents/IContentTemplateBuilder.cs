using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Framework.Foundation;

public interface IContentTemplateBuilder
{
    IReadOnlyCollection<IContentTemplateDescriptor> Descriptors { get; }

    IContentTemplateBuilder Add<TViewModel, TView>(string name, ServiceLifetime lifetime = ServiceLifetime.Transient);

    IContentTemplateBuilder Add<TViewModel, TView>(ServiceLifetime lifetime = ServiceLifetime.Transient);
}