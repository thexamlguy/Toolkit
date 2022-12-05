using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation
{
    public interface ITemplateDescriptor
    {
        Type DataType { get; }

        ServiceLifetime Lifetime { get; }

        string? Name { get; }

        Type TemplateType { get; }
    }
}
