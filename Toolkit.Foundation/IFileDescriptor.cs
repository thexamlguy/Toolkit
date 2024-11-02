namespace Toolkit.Foundation;

public interface IFileDescriptor
{
    string Name { get; } 

    string Path { get; }

    long Size { get; }
}
