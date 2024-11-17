using CommunityToolkit.Mvvm.Messaging;
using Toolkit.Foundation;
using Windows.Win32;

namespace Toolkit.Windows;

public class Taskbar(IMessenger messenger,
    IDisposer disposer) :
    ITaskbar,
    IRecipient<WndProcEventArgs>,
    IRecipient<PointerReleasedEventArgs>,
    IRecipient<PointerMovedEventArgs>,
    IRecipient<PointerDragEventArgs>
{
    private bool isDrag;
    private bool isWithinBounds;

    public void Dispose()
    {
        messenger.UnregisterAll(this);
        disposer.Dispose(this);

        GC.SuppressFinalize(this);
    }

    public TaskbarState GetCurrentState()
    {
        nint handle = GetHandle();
        TaskbarState state = new TaskbarState
        {
            Screen = Screen.FromHandle(handle)
        };

        PInvoke.AppBarData appBarData = PInvoke.GetAppBarData(handle);
        PInvoke.GetAppBarPosition(ref appBarData);

        state.Rect = appBarData.rect.ToRect();
        state.Placement = (TaskbarPlacement)appBarData.uEdge;

        return state;
    }

    public IntPtr GetHandle() => WindowHelper.FindWindow("Shell_TrayWnd");

    public void Receive(WndProcEventArgs args)
    {
        if (args.Message == PInvoke.WM_TASKBARCREATED ||
            args.Message == (int)WndProcMessages.WM_SETTINGCHANGE &&
            (int)args.WParam == PInvoke.SPI_SETWORKAREA)
        {
            messenger.Send<TaskbarChangedEventArgs>();
        }
    }

    public void Receive(PointerReleasedEventArgs args)
    {
        if (isDrag)
        {
            isDrag = false;
        }
    }

    public void Receive(PointerMovedEventArgs args)
    {
        nint taskbarHandle = GetHandle();
        if (WindowHelper.TryGetWindowBounds(taskbarHandle, out Rect? rect))
        {
            if (args.Location.IsWithinBounds(rect))
            {
                if (isWithinBounds)
                {
                    return;
                }

                isWithinBounds = true;
                messenger.Send<TaskbarEnteredEventArgs>();
            }
            else
            {
                isDrag = false;
                isWithinBounds = false;
            }
        }
    }

    public void Receive(PointerDragEventArgs args)
    {
        if (isWithinBounds)
        {
            if (isDrag)
            {
                messenger.Send<TaskbarDragOverEventArgs>();
            }
            else
            {
                messenger.Send<TaskbarDragEnterEventArgs>();
            }

            isDrag = true;
        }
        else
        {
            isDrag = false;
        }
    }

    public void Initialize() => messenger.RegisterAll(this);
}
