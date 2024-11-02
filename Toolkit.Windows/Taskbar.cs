using Toolkit.Foundation;
using Windows.Win32;

namespace Toolkit.Windows;

public class Taskbar(ISubscriber subscriber,
    IPublisher publisher,
    IDisposer disposer) :
    ITaskbar,
    INotificationHandler<WndProcEventArgs>,
    INotificationHandler<PointerReleasedEventArgs>,
    INotificationHandler<PointerMovedEventArgs>,
    INotificationHandler<PointerDragEventArgs>
{
    private bool isDrag;
    private bool isWithinBounds;

    public void Dispose()
    {
        disposer.Dispose(this);
        GC.SuppressFinalize(this);
    }

    public TaskbarState GetCurrentState()
    {
        var handle = GetHandle();
        var state = new TaskbarState
        {
            Screen = Screen.FromHandle(handle)
        };

        var appBarData = PInvoke.GetAppBarData(handle);
        PInvoke.GetAppBarPosition(ref appBarData);

        state.Rect = appBarData.rect.ToRect();
        state.Placement = (TaskbarPlacement)appBarData.uEdge;

        return state;
    }

    public IntPtr GetHandle() => WindowHelper.Find("Shell_TrayWnd");

    public Task Handle(WndProcEventArgs args)
    {
        if (args.Message == PInvoke.WM_TASKBARCREATED ||
            args.Message == (int)WndProcMessages.WM_SETTINGCHANGE &&
            (int)args.WParam == PInvoke.SPI_SETWORKAREA)
        {
            publisher.Publish<TaskbarChangedEventArgs>();
        }

        return Task.CompletedTask;
    }

    public Task Handle(PointerReleasedEventArgs args)
    {
        if (isDrag)
        {
            isDrag = false;
        }

        return Task.CompletedTask;
    }

    public Task Handle(PointerMovedEventArgs args)
    {
        nint taskbarHandle = GetHandle();
        if (WindowHelper.TryGetBounds(taskbarHandle, out var rect))
        {
            if (args.Location.IsWithinBounds(rect))
            {
                if (isWithinBounds)
                {
                    return Task.CompletedTask;
                }

                isWithinBounds = true;
                publisher.Publish<TaskbarEnteredEventArgs>();
            }
            else
            {
                isDrag = false;
                isWithinBounds = false;
            }
        }

        return Task.CompletedTask;
    }

    public Task Handle(PointerDragEventArgs args)
    {
        if (isWithinBounds)
        {
            if (isDrag)
            {
                publisher.Publish<TaskbarDragOverEventArgs>();
            }
            else
            {
                publisher.Publish<TaskbarDragEnterEventArgs>();
            }

            isDrag = true;
        }
        else
        {
            isDrag = false;
        }

        return Task.CompletedTask;
    }

    public void Initialize() => subscriber.Subscribe(this);
}
