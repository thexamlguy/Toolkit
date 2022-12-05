using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Toolkit.Foundation.Avalonia
{
    public class ContentControlHandler : NavigationRouteHandler<ContentControl>
    {
        public override void Receive(NavigationRouteRequest<ContentControl> message)
        {
            if (message.Template is TemplatedControl control)
            {
                control.DataContext = message.Data;
                message.Target.Content = control;
            }

            message.Reply(true);
        }
    }
}
