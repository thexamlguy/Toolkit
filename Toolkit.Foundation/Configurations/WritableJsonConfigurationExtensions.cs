using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Toolkit.Foundation
{
    public static class WritableJsonConfigurationExtensions
    {
        public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder,
            string path)
        {
            return builder.AddWritableJsonFile(null, path, false, false, null);
        }

        public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder,
            string path,
            Action<IWritableJsonConfigurationBuilder>? factoryDelegate)
        {
            return builder.AddWritableJsonFile(null, path, false, false, factoryDelegate);
        }

        public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder,
            string path,
            bool optional)
        {
            return builder.AddWritableJsonFile(null, path, optional, false, null);
        }

        public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder,
            string path,
            bool optional,
            Action<IWritableJsonConfigurationBuilder>? factoryDelegate)
        {
            return builder.AddWritableJsonFile(null, path, optional, false, factoryDelegate);
        }

        public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder,
            string path,
            bool optional,
            bool reloadOnChange)
        {
            return builder.AddWritableJsonFile(null, path, optional, reloadOnChange, null);
        }

        public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder,
            string path,
            bool optional,
            bool reloadOnChange,
            Action<IWritableJsonConfigurationBuilder>? factoryDelegate)
        {
            return builder.AddWritableJsonFile(null, path, optional, reloadOnChange, factoryDelegate);
        }

        public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder,
            IFileProvider? provider,
            string path,
            bool optional,
            bool reloadOnChange, Action<IWritableJsonConfigurationBuilder>? writableJsonConfigurationDelegate)
        {
            IWritableJsonConfigurationBuilder writableJsonConfigurationBuilder = new WritableJsonConfigurationBuilder();
            writableJsonConfigurationDelegate?.Invoke(writableJsonConfigurationBuilder);

            return builder.AddWritableJsonFile(configuration =>
            {
                configuration.FileProvider = provider;
                configuration.Path = path;
                configuration.Optional = optional;
                configuration.ReloadOnChange = reloadOnChange;
                configuration.Factory = writableJsonConfigurationBuilder;
                configuration.ResolveFileProvider();
            });
        }

        public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder, Action<WritableJsonConfigurationSource> configureSource) => builder.Add(configureSource);
    }

}
