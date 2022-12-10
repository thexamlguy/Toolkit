using System.Reflection;

namespace Toolkit.Framework.Foundation;

internal class ServiceFactoryDescriptor
{
    private readonly IServiceFactory serviceFactory;

    public ServiceFactoryDescriptor(IServiceFactory serviceFactory)
    {
        this.serviceFactory = serviceFactory;
    }

    public object? Create(Type type)
    {
        MethodInfo? methodInfo = typeof(ServiceFactoryDescriptor).GetMethod(nameof(Create), BindingFlags.NonPublic | BindingFlags.Instance);
        return methodInfo?.MakeGenericMethod(type).Invoke(this, new object[] { type });
    }

    private T? Create<T>(Type type)
    {
        return serviceFactory.Create<T>(type);
    }
}