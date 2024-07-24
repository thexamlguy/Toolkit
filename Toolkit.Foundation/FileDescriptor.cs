namespace Toolkit.Foundation;

public record FileDescriptor(string Name, string Path, int Size) :
    IFileDescriptor;