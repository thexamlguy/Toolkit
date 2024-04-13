using Avalonia.Threading;

namespace Toolkit.UI.Controls.Avalonia;
public class ScopedBatchHelper
{
    private DispatcherTimer? timer;

    public Action? Completed { get; set; }

    public void Start(TimeSpan duration)
    {
        timer ??= new DispatcherTimer(duration, DispatcherPriority.Background, Tick);
        timer.Start();
    }

    private void Tick(object? sender, EventArgs args)
    {
        timer?.Stop();
        Completed?.Invoke();
        Completed = null;
    }
}