namespace Toolkit.Foundation;

public interface IServiceScopeFactory<TService>
{
    TService? Create(params object?[] parameters);
}