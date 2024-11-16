using CommunityToolkit.Mvvm.Messaging;
using System.Runtime.InteropServices;
using Toolkit.Foundation;

namespace Toolkit.Windows;


public partial class NotifyIcon(IWndProc wndProc,
    IMessenger messenger) :
    INotifyIcon,
    IRecipient<WndProcEventArgs>
{
    private const int CallbackMessage = 0x400;
    private const uint IconVersion = 0x4;

    private readonly Lock notifyLock = new();
    private bool isDisposed;
    private NotifyIconData notifyIconData;

    ~NotifyIcon()
    {
        Dispose(false);
    }

    private enum NotifyIconBalloonType
    {
        None = 0x00,
        Info = 0x01,
        Warning = 0x02,
        Error = 0x03,
        User = 0x04,
        NoSound = 0x10,
        LargeIcon = 0x20,
        RespectQuietTime = 0x80
    }

    private enum NotifyIconCommand : uint
    {
        Add = 0x0,
        Delete = 0x2,
        Modify = 0x1,
        SetVersion = 0x4
    }

    [Flags]
    private enum NotifyIconDataMember : uint
    {
        Message = 0x01,
        Icon = 0x02,
        Tip = 0x04,
        State = 0x08,
        Info = 0x10,
        Realtime = 0x40,
        UseLegacyToolTips = 0x80
    }

    private enum NotifyIconState : uint
    {
        Visible = 0x00,
        Hidden = 0x01
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Initialize()
    {
        messenger.RegisterAll(this);
        CreateNotificationIcon();
    }

    public void Receive(WndProcEventArgs message)
    {
        if (message.Message == CallbackMessage)
        {
            switch (message.LParam)
            {
                case (uint)WndProcMessages.WM_LBUTTONUP:
                    messenger.Send(new NotifyIconInvokedEventArgs(PointerButton.Left));
                    break;

                case (uint)WndProcMessages.WM_MBUTTONUP:
                    messenger.Send(new NotifyIconInvokedEventArgs(PointerButton.Middle));
                    break;

                case (uint)WndProcMessages.WM_RBUTTONUP:
                    messenger.Send(new NotifyIconInvokedEventArgs(PointerButton.Right));
                    break;
            }
        }
    }

    public void SetIcon(IntPtr iconHandle)
    {
        lock (notifyLock)
        {
            notifyIconData.IconHandle = iconHandle;
            WriteNotifyIconData(NotifyIconCommand.Modify, NotifyIconDataMember.Icon);
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr DefWindowProcW(IntPtr handle, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("shell32.dll", SetLastError = true)]
    private static extern int Shell_NotifyIcon(NotifyIconCommand notifyCommand, ref NotifyIconData notifyIconData);

    private void CreateNotificationIcon()
    {
        lock (notifyLock)
        {
            notifyIconData = new NotifyIconData();

            notifyIconData.cbSize = (uint)Marshal.SizeOf(notifyIconData);
            notifyIconData.WindowHandle = wndProc.Handle;
            notifyIconData.TaskbarIconId = 0x0;
            notifyIconData.CallbackMessageId = CallbackMessage;
            notifyIconData.VersionOrTimeout = IconVersion;

            notifyIconData.IconHandle = IntPtr.Zero;

            notifyIconData.IconState = NotifyIconState.Hidden;
            notifyIconData.StateMask = NotifyIconState.Hidden;

            WriteNotifyIconData(NotifyIconCommand.Add, NotifyIconDataMember.Message | NotifyIconDataMember.Icon | NotifyIconDataMember.Tip);
        }
    }

    private void Dispose(bool disposing)
    {
        if (isDisposed || !disposing) return;
        lock (notifyLock)
        {
            isDisposed = true;

            messenger.UnregisterAll(this);
            RemoveNotificationIcon();
        }
    }

    private void RemoveNotificationIcon() => WriteNotifyIconData(NotifyIconCommand.Delete, NotifyIconDataMember.Message);

    private void WriteNotifyIconData(NotifyIconCommand command, NotifyIconDataMember flags)
    {
        notifyIconData.ValidMembers = flags;
        lock (notifyLock)
        {
            Shell_NotifyIcon(command, ref notifyIconData);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct NotifyIconData
    {
        public uint cbSize;
        public IntPtr WindowHandle;
        public uint TaskbarIconId;
        public NotifyIconDataMember ValidMembers;
        public uint CallbackMessageId;
        public IntPtr IconHandle;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string ToolTipText;

        public NotifyIconState IconState;
        public NotifyIconState StateMask;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string BalloonText;

        public uint VersionOrTimeout;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string BalloonTitle;

        public NotifyIconBalloonType BalloonFlags;
        public Guid TaskbarIconGuid;
        public IntPtr CustomBalloonIconHandle;
    }
}