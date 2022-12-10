using Avalonia;
using Avalonia.Markup.Xaml;
using System.Reflection;

namespace Toolkit.Framework.Avalonia;

public class TriggerExtension : MarkupExtension
{
    public AvaloniaObject? TargetObject { get; protected set; }

    protected object? TargetInvoke { get; private set; }

    public void Invoke(object sender, EventArgs args)
    {
        OnInvoked(sender, args);
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target)
        {
            if (target.TargetObject is AvaloniaObject avaloniaObject)
            {
                TargetObject = avaloniaObject;
            }
            else if (serviceProvider.GetService(typeof(IRootObjectProvider)) is IRootObjectProvider root)
            {
                TargetObject = (AvaloniaObject)root.RootObject;
            }

            if (TargetObject is not null)
            {
                string? targetName = target.TargetProperty as string;

                TargetInvoke = target.TargetProperty;
                OnAttached(serviceProvider);

                EventInfo? eventInfo = target.TargetProperty as EventInfo ?? (targetName is not null ? TargetObject.GetType().GetEvent(targetName) : null);
                MethodInfo? methodInfo = eventInfo is not null ? null : target.TargetProperty as MethodInfo ?? (targetName is not null ? TargetObject.GetType().GetMethod(targetName) : null);

                MethodInfo invokeMethod = GetType().GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public)!;
                if (invokeMethod is null)
                {
                    return this;
                }

                if (eventInfo is not null)
                {
                    return Delegate.CreateDelegate(eventInfo.EventHandlerType!, this, invokeMethod);
                }

                if (methodInfo is not null)
                {
                    if (methodInfo.GetParameters() is ParameterInfo[] methodParameters && methodParameters is { Length: 2 })
                    {
                        return Delegate.CreateDelegate(methodParameters[1].ParameterType, this, invokeMethod);
                    }
                }
            }
        }

        return null;
    }

    protected virtual void OnAttached(IServiceProvider serviceProvider)
    {
    }

    protected virtual void OnInvoked(object sender, EventArgs args)
    {
    }
}