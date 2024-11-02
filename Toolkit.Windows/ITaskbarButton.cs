namespace Toolkit.Windows;

public interface ITaskbarButton : 
    IDisposable
{
    Rect Rect { get; }

    string Name { get; }
}
