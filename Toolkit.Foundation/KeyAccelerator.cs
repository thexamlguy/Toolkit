namespace Toolkit.Foundation;

public record KeyAccelerator(VirtualKey Key,
    VirtualKey[]? Modifiers = null) :
    IRequest;
