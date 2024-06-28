using Avalonia.Controls;

namespace Toolkit.Avalonia;

public interface ITopLevelProvider
{
    TopLevel? Get();
}
