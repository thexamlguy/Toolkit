namespace Toolkit.Foundation;

public interface IImageReader
{
    Task<IImageDescriptor> Get(Stream stream, 
        int width, 
        int height, 
        bool maintainAspectRatio = false);
}
