using CommunityToolkit.Mvvm.Messaging;
using Toolkit.Foundation;

namespace Toolkit.Windows;

public class TaskbarButton : 
    ITaskbarButton,
    IRecipient<PointerReleasedEventArgs>,
    IRecipient<PointerMovedEventArgs>,
    IRecipient<PointerDragEventArgs>
{
    private readonly IMessenger messenger;
    private readonly IDisposer disposer;
    private bool isWithinBounds;
    private bool isDrag;

    public TaskbarButton(string name,
        Rect rect,
        IMessenger messenger,
        IDisposer disposer)
    {
        this.messenger = messenger;
        this.disposer = disposer;

        Name = name;
        Rect = rect;

        messenger.RegisterAll(this);
    }

    public Rect Rect { get; internal set; }

    public string Name { get; internal set; }

    public void Dispose()
    {
        messenger.UnregisterAll(this);
        disposer.Dispose(this);

        GC.SuppressFinalize(this);
    }

    public void Receive(PointerReleasedEventArgs args)
    {
        if (!isDrag && isWithinBounds)
        {
            messenger.Send(new TaskbarButtonInvokedEventArgs(this));
        }

        if (isDrag)
        {
            isDrag = false;
        }
    }

    public void Receive(PointerDragEventArgs args)
    {
        if (isWithinBounds)
        {
            if (isDrag)
            {
                messenger.Send(new TaskbarButtonDragOverEventArgs(this));
            }
            else
            {
                messenger.Send(new TaskbarButtonDragEnterEventArgs(this));
            }

            isDrag = true;
        }
        else
        {
            isDrag = false;
        }
    }

    public void Receive(PointerMovedEventArgs args)
    {
        if (args.Location.IsWithinBounds(Rect))
        {
            if (isWithinBounds)
            {
                return;
            }

            isWithinBounds = true;
            messenger.Send(new TaskbarButtonEnteredEventArgs(this));
        }
        else
        {
            isDrag = false;
            isWithinBounds = false;
        }
    }
}
