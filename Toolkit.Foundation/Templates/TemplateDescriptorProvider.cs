namespace Toolkit.Foundation
{
    public class TemplateDescriptorProvider : ITemplateDescriptorProvider
    {
        private readonly IReadOnlyCollection<ITemplateDescriptor> descriptors;

        public TemplateDescriptorProvider(IReadOnlyCollection<ITemplateDescriptor> descriptors)
        {
            this.descriptors = descriptors;
        }

        public ITemplateDescriptor? Get(string name)
        {
            if (descriptors.FirstOrDefault(x => x.Name == name) is ITemplateDescriptor descriptor)
            {
                return descriptor;
            }

            return null;
        }

        public ITemplateDescriptor? Get(Type type)
        {
            if (descriptors.FirstOrDefault(x => x.DataType == type) is ITemplateDescriptor descriptor)
            {
                return descriptor;
            }

            return null;
        }

        public ITemplateDescriptor? Get<T>()
        {
            if (descriptors.FirstOrDefault(x => x.DataType == typeof(T)) is ITemplateDescriptor descriptor)
            {
                return descriptor;
            }

            return null;
        }
    }
}
