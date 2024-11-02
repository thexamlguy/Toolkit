namespace Toolkit.Foundation;

public record FileDescriptor(string Name, string Path, long Size) :
    IFileDescriptor;