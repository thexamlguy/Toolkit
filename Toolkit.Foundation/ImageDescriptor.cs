namespace Toolkit.Foundation;

public record ImageDescriptor(object Image, double Width, double Height) :
    IImageDescriptor;