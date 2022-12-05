using Avalonia.Controls.Primitives;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Navigation;

namespace Toolkit.Foundation.Avalonia
{
    public class FrameHandler : NavigationRouteHandler<Frame>
    {
        public override async void Receive(NavigationRouteRequest<Frame> message)
        {
            message.Target.NavigationPageFactory = new NavigationPageFactory();

            TaskCompletionSource<bool> completionSource = new();
            if (message.Template is TemplatedControl content)
            {
                void HandleNavigated(object sender, NavigationEventArgs args)
                {
                    message.Target.Navigated -= HandleNavigated;
                    if (message.Target.Content is TemplatedControl control)
                    {
                        control.DataContext = message.Data;
                        completionSource.SetResult(true);
                    }
                }

                message.Target.Navigated += HandleNavigated;
                message.Target.NavigateFromObject(content);
            }

            bool result = await completionSource.Task;
            message.Reply(result);
        }
    }
}
