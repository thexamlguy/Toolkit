namespace Toolkit.Avalonia;

public interface ITransientNavigationStore<TControl> 
    where TControl : class
{
    void Clear();

    T? Get<T>(TControl control)
           where T : class;
  
    void Remove(TControl control);

    void Set(TControl control,
        object parameters);
}