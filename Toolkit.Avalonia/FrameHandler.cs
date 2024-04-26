using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using FluentAvalonia.UI.Navigation;
using System.Reflection;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public class FrameHandler(INavigationContext navigationContext) :
    INavigateHandler<Frame>,
    INavigateBackHandler<Frame>
{
    public Task Handle(Navigate<Frame> args,
        CancellationToken cancellationToken)
    {
        if (args.Context is Frame frame)
        {
            frame.NavigationPageFactory ??= new NavigationPageFactory();
            if (args.Template is Control control)
            {
                void NavigatingFrom(object? sender,
                    Control control)
                {
                    async void HandleNavigatingFrom(object? _,
                        NavigatingCancelEventArgs args)
                    {
                        Dictionary<string, object> results = [];

                        control.RemoveHandler(Frame.NavigatingFromEvent, HandleNavigatingFrom);
                        NavigatedFrom(sender, control, () => results);

                        if (control.DataContext is object content)
                        {
                            if (content is IConfirmation confirmation &&
                                !await confirmation.Confirm())
                            {
                                args.Cancel = true;
                            }

                            if (!args.Cancel)
                            {
                                if (content is IDeactivating deactivating)
                                {
                                    await deactivating.Deactivating();
                                }

                                Type contentType = content.GetType();
                                if (contentType.GetInterfaces() is Type[] contracts)
                                {
                                    foreach (Type contract in contracts)
                                    {
                                        if (contract.Name == typeof(IDeactivating<>).Name &&
                                            contract.GetGenericArguments() is { Length: 1 } arguments)
                                        {
                                            if (contentType.GetMethods().FirstOrDefault(x =>
                                                x.Name == "Deactivating" && x.ReturnType == typeof(Task<>)
                                                    .MakeGenericType(arguments[0]))
                                                        is MethodInfo methodInfo)
                                            {
                                                if (methodInfo.GetCustomAttribute<NavigationContextAttribute>()
                                                    is NavigationContextAttribute attribute)
                                                {
                                                    if (await methodInfo.InvokeAsync<object?>(content) is object result)
                                                    {
                                                        results.Add(attribute.Name, result);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    control.AddHandler(Frame.NavigatingFromEvent, HandleNavigatingFrom);
                }

                void NavigatedFrom(object? sender,
                    Control control,
                    Func<Dictionary<string, object>> resultCallBack)
                {
                    async void HandleNavigatedFrom(object? _,
                        NavigationEventArgs args)
                    {
                        control.RemoveHandler(Frame.NavigatedFromEvent, HandleNavigatedFrom);
                        if (args.NavigationMode == NavigationMode.New)
                        {
                            NavigatedTo(sender, control);
                        }

                        Dictionary<string, object> results = resultCallBack.Invoke();
                        async Task DoNavigatedFromAsync(object? content)
                        {
                            if (content is not null)
                            {
                                if (content is IDeactivated deactivated)
                                {
                                    await deactivated.Deactivated();
                                }

                                Type contentType = content.GetType();
                                if (contentType.GetInterfaces() is Type[] contracts)
                                {
                                    foreach (Type contract in contracts)
                                    {
                                        if (contract.Name == typeof(IActivated<>).Name &&
                                            contract.GetGenericArguments() is { Length: 1 } arguments)
                                        {
                                            if (contentType.GetMethods().FirstOrDefault(x =>
                                                x.Name == "NavigatedToAsync" &&
                                                    x.GetCustomAttribute<NavigationContextAttribute>()
                                                    is NavigationContextAttribute attribute && results.ContainsKey(attribute.Name))
                                                is MethodInfo methodInfo)
                                            {
                                                if (methodInfo.GetCustomAttribute<NavigationContextAttribute>()
                                                    is NavigationContextAttribute attribute)
                                                {
                                                    if (results.TryGetValue(attribute.Name, out object? value))
                                                    {
                                                        await methodInfo.InvokeAsync(content, value);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (args.Source is TemplatedControl sourceTemplate)
                        {
                            if (sourceTemplate.DataContext is object content)
                            {
                                await DoNavigatedFromAsync(content);
                            }
                        }

                        if (sender is TemplatedControl senderTemplate)
                        {
                            if (senderTemplate.DataContext is object content)
                            {
                                await DoNavigatedFromAsync(content);
                            }
                        }
                        else
                        {
                            await DoNavigatedFromAsync(sender);
                        }
                    }

                    control.AddHandler(Frame.NavigatedFromEvent, HandleNavigatedFrom);
                }

                void NavigatedTo(object? sender,
                    Control control)
                {
                    async void HandleNavigatedTo(object? _,
                        NavigationEventArgs __)
                    {
                        control.RemoveHandler(Frame.NavigatedToEvent, HandleNavigatedTo);
                        NavigatingFrom(sender, control);

                        if (control.DataContext is object content)
                        {
                            if (content is IInitializer initializer)
                            {
                                await initializer.Initialize();
                            }

                            if (content is IActivated activated)
                            {
                                await activated.Activated();
                            }
                        }
                    }

                    control.AddHandler(Frame.NavigatedToEvent, HandleNavigatedTo);
                }

                control.DataContext = args.Content;
                navigationContext.Set(control);

                NavigatedTo(args.Sender, control);
                frame.NavigateFromObject(control);
            }
        }

        return Task.CompletedTask;
    }

    public Task Handle(NavigateBack<Frame> args,
        CancellationToken cancellationToken = default)
    {
        if (args.Context is Frame frame)
        {
            frame.GoBack();
        }

        return Task.CompletedTask;
    }
}