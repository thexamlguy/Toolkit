using Toolkit.Foundation;

namespace Toolkit.Windows;

public class TaskbarButton : 
    ITaskbarButton,
    INotificationHandler<PointerReleasedEventArgs>,
    INotificationHandler<PointerMovedEventArgs>,
    INotificationHandler<PointerDragEventArgs>
{
    private readonly IPublisher publisher;
    private readonly IDisposer disposer;
    private bool isWithinBounds;
    private bool isDrag;

    public TaskbarButton(string name,
        Rect rect,
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer)
    {
        this.publisher = publisher;
        this.disposer = disposer;

        Name = name;
        Rect = rect;

        subscriber.Subscribe(this);
    }

    public Rect Rect { get; internal set; }

    public string Name { get; internal set; }

    public void Dispose()
    {
        disposer.Dispose(this);
        GC.SuppressFinalize(this);
    }

    public Task Handle(PointerReleasedEventArgs args)
    {
        if (!isDrag && isWithinBounds)
        {
            publisher.Publish(new TaskbarButtonInvokedEventArgs(this));
        }

        if (isDrag)
        {
            isDrag = false;
        }

        return Task.CompletedTask;
    }

    public Task Handle(PointerDragEventArgs args)
    {
        if (isWithinBounds)
        {
            if (isDrag)
            {
                publisher.Publish(new TaskbarButtonDragOverEventArgs(this));
            }
            else
            {
                publisher.Publish(new TaskbarButtonDragEnterEventArgs(this));
            }

            isDrag = true;
        }
        else
        {
            isDrag = false;
        }

        return Task.CompletedTask;
    }

    public Task Handle(PointerMovedEventArgs args)
    {
        if (args.Location.IsWithinBounds(Rect))
        {
            if (isWithinBounds)
            {
                return Task.CompletedTask;
            }

            isWithinBounds = true;
            publisher.Publish(new TaskbarButtonEnteredEventArgs(this));
        }
        else
        {
            isDrag = false;
            isWithinBounds = false;
        }

        return Task.CompletedTask;
    }
}
