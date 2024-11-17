using CommunityToolkit.Mvvm.Messaging;
using Toolkit.Foundation;
using UIAutomationClient;

namespace Toolkit.Windows;

public class TaskbarButtonMonitor : 
    ITaskbarButtonMonitor
{
    private readonly IDispatcherTimer dispatcherTimer;
    private readonly IDispatcherTimerFactory dispatcherTimerFactory;
    private readonly IDisposer disposer;
    private readonly IMessenger messenger;
    private readonly IServiceFactory serviceFactory;
    private readonly Dictionary<string, TaskbarButton> taskbarButtons = [];
    private readonly ITaskbarList taskbarList;
    private Rect? taskbarRectCache;
    private IUIAutomationCondition? taskListCondition;
    private IUIAutomationElement? taskListElement;
    private IntPtr taskListHandle;

    public TaskbarButtonMonitor(ITaskbarList taskbarList,
        IMessenger messenger,
        IDispatcherTimerFactory dispatcherTimerFactory,
        IServiceFactory serviceFactory,
        IDisposer disposer)
    {
        this.taskbarList = taskbarList;
        this.messenger = messenger;
        this.dispatcherTimerFactory = dispatcherTimerFactory;
        this.serviceFactory = serviceFactory;
        this.disposer = disposer;

        disposer.Add(this, dispatcherTimer = dispatcherTimerFactory.Create(OnDispatcher, 
            TimeSpan.FromMilliseconds(500)));
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        disposer.Dispose(this);
    }

    public void Initialize()
    {
        CUIAutomation clientUIAutomation = new();
        taskListCondition = clientUIAutomation.CreateTrueCondition();

        taskListHandle = taskbarList.GetHandle();
        taskListElement = clientUIAutomation.ElementFromHandle(taskListHandle);

        if (WindowHelper.TryGetWindowBounds(taskListHandle, out Rect? rect))
        {
            taskbarRectCache = rect;
        }

        dispatcherTimer.Start();
        UpdateTaskbarButtons();
    }

    private bool CheckDirtyTaskbarRegion()
    {
        if (WindowHelper.TryGetWindowBounds(taskListHandle, out Rect? rect))
        {
            if (taskbarRectCache?.Width != rect.Width ||
                taskbarRectCache?.Height != rect.Height)
            {
                taskbarRectCache = rect;
                return true;
            }
        }

        return false;
    }

    private Dictionary<string, tagRECT> FindTaskbarButtons()
    {
        IUIAutomationElementArray? taskElements = taskListElement?.FindAll(TreeScope.TreeScope_Descendants |
            TreeScope.TreeScope_Children, taskListCondition);

        Dictionary<string, tagRECT> buttons = [];
        if (taskElements is not null)
        {
            for (int index = 0; index <= taskElements.Length - 1; index++)
            {
                IUIAutomationElement taskUIElement = taskElements.GetElement(index);
                string name = taskUIElement.CurrentName;
                tagRECT rect = taskUIElement.CurrentBoundingRectangle;

                buttons.Add(name, rect);
            }
        }

        return buttons;
    }

    private void OnDispatcher()
    {
        dispatcherTimer.Stop();

        if (CheckDirtyTaskbarRegion())
        {
            UpdateTaskbarButtons();
        }

        dispatcherTimer.Start();
    }

    private void UpdateTaskbarButtons()
    {
        if (taskListElement is null)
        {
            return;
        }

        Dictionary<string, tagRECT> buttons = FindTaskbarButtons();

        foreach (KeyValuePair<string, TaskbarButton> buttonToRemove in taskbarButtons
            .Where(taskbarButton => !buttons.ContainsKey(taskbarButton.Key)))
        {
            string key = buttonToRemove.Key;
            TaskbarButton button = buttonToRemove.Value;

            taskbarButtons.Remove(key);
            messenger.Send(new TaskbarButtonRemovedEventArgs(button));

            button.Dispose();
        }

        foreach (KeyValuePair<string, tagRECT> button in buttons)
        {
            string name = button.Key;
            tagRECT bounds = button.Value;

            Rect rect = new(bounds.left,
                bounds.top,
                bounds.right - bounds.left,
                bounds.bottom - bounds.top);

            if (taskbarButtons.TryGetValue(name, out TaskbarButton? taskbarButton))
            {
                taskbarButtons[name].Rect = rect;
                messenger.Send(new TaskbarButtonUpdatedEventArgs(taskbarButtons[name]));
            }
            else
            {
                taskbarButtons.Add(name, serviceFactory.Create<TaskbarButton>(name, rect));
                messenger.Send(new TaskbarButtonCreatedEventArgs(taskbarButtons[name]));
            }
        }
    }
}
