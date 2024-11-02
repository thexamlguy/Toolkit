namespace Toolkit.Foundation
{
    public interface IConfigurationCache
    {
        bool Remove(string section);
        void Set<TConfiguration>(string section, TConfiguration configuration);
        bool TryGet<TConfiguration>(string section, out TConfiguration? configuration);
    }
}