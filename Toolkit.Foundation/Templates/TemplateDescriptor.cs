using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation
{
    public class TemplateDescriptor : ITemplateDescriptor
    {
        public TemplateDescriptor(Type dataType,
            Type templateType,
            string? name = null,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            TemplateType = templateType;
            DataType = dataType;
            Name = name;
            Lifetime = lifetime;
        }

        public ServiceLifetime Lifetime { get; }

        public Type TemplateType { get; }

        public Type DataType { get; }

        public string? Name { get; }
    }
}
