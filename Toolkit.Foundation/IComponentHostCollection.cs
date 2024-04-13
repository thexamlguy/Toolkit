namespace Toolkit.Foundation;

public interface IComponentHostCollection :
    IEnumerable<IComponentHost>
{
    void Add(IComponentHost host);
}