namespace Toolkit.Foundation;

public record ImageDescriptor(object Image, int Width, int Height) :
    IImageDescriptor;