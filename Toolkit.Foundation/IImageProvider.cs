namespace Toolkit.Foundation;

public interface IImageProvider
{
    Task<IImageDescriptor> Get(string filePath, 
        int width, 
        int height, 
        bool maintainAspectRatio = false);
}
