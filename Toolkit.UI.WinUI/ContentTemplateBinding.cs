using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Toolkit.Foundation;

namespace Toolkit.UI.WinUI;

[Bindable]
public static class ContentTemplateBinding
{
    public static readonly DependencyProperty IsAttachedProperty =
        DependencyProperty.RegisterAttached("IsAttached",
            typeof(bool), typeof(ContentTemplateBinding),
                new PropertyMetadata(false, OnAttachLoadedEventChanged));

    public static bool GetIsAttached(DependencyObject dependencyObject) =>
        (bool)dependencyObject.GetValue(IsAttachedProperty);

    public static void SetIsAttached(DependencyObject
        dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);

    private static void OnAttachLoadedEventChanged(DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs args)
    {
        if (dependencyObject is FrameworkElement content)
        {
            IActivation? cachedActivation = null;

            void HandleLoaded(object sender, RoutedEventArgs args)
            {
                if (sender is FrameworkElement content)
                {
                    content.Loaded -= HandleLoaded;

                    if (content.DataContext is IActivation activation)
                    {
                        cachedActivation = activation;
                        activation.IsActive = true;
                    }
                }
            }

            void HandleUnloaded(object sender, RoutedEventArgs args)
            {
                if (cachedActivation is not null)
                {
                    cachedActivation.IsActive = false;
                }

                if (sender is FrameworkElement content)
                {
                    cachedActivation = null;
                    content.Unloaded -= HandleUnloaded;
                }
            }

            if ((bool)args.NewValue)
            {
                content.Loaded += HandleLoaded;
                content.Unloaded += HandleUnloaded;
            }
            else
            {
                content.Loaded -= HandleLoaded;
                content.Unloaded -= HandleUnloaded;
            }
        }
    }
}