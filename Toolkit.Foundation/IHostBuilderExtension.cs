﻿using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Toolkit.Foundation;

public static class IHostBuilderExtension
{
    public static IHostBuilder AddConfiguration<TConfiguration>(this IHostBuilder builder,
        string section)
        where TConfiguration : class, new() =>
            builder.AddConfiguration<TConfiguration>(section, "Settings.json", null);

    public static IHostBuilder AddConfiguration<TConfiguration>(this IHostBuilder services)
        where TConfiguration : class, new() =>
            services.AddConfiguration<TConfiguration>(typeof(TConfiguration).Name, "Settings.json", null);

    public static IHostBuilder AddConfiguration<TConfiguration>(this IHostBuilder builder,
        string section,
        TConfiguration configuration)
        where TConfiguration : class, new()
    {
        return builder.AddConfiguration(section, "Settings.json", configuration);
    }

    public static IHostBuilder AddConfiguration<TConfiguration>(this IHostBuilder builder,
        string section,
        Action<TConfiguration> configurationDelegate)
        where TConfiguration : class, new()
    {
        TConfiguration configuration = new();
        configurationDelegate.Invoke(configuration);

        return builder.AddConfiguration(section, "Settings.json", configuration);
    }

    public static IHostBuilder AddConfiguration<TConfiguration>(this IHostBuilder builder,
        Action<TConfiguration> configurationDelegate,
        string section)
        where TConfiguration : class, new()
    {
        TConfiguration configuration = new();
        configurationDelegate.Invoke(configuration);

        return builder.AddConfiguration(section, "Settings.json", configuration);
    }

    public static IHostBuilder AddConfiguration<TConfiguration>(this IHostBuilder builder,
        TConfiguration configuration)
        where TConfiguration : class, new() =>
            builder.AddConfiguration(configuration.GetType().Name, "Settings.json", configuration);

    public static IHostBuilder AddConfiguration<TConfiguration>(this IHostBuilder builder,
        object configuration)
        where TConfiguration : class, new() =>
            builder.AddConfiguration(configuration.GetType().Name,
                "Settings.json", (TConfiguration?)configuration);

    public static IHostBuilder AddConfiguration<TConfiguration>(this IHostBuilder builder,
        string section,
        string path = "Settings.json",
        TConfiguration? defaultConfiguration = null,
        Action<JsonSerializerOptions>? serializerDelegate = null)
        where TConfiguration : class, new()
    {
        builder.ConfigureServices((context, services) =>
        {
            HashSet<string> sections = [];
            if (section.EndsWith(":*"))
            {
                section = section[..^1];
                if (context.Configuration is ConfigurationRoot root)
                {
                    foreach (KeyValuePair<string, string?> configuration in root.AsEnumerable())
                    {
                        string[] segments = configuration.Key.Split(':');
                        if (segments.Length > 2)
                        {
                            string keyPrefix = string.Join(':', segments.Take(2));
                            if (keyPrefix.StartsWith(section) && !keyPrefix.EndsWith(":*"))
                            {
                                sections.Add(keyPrefix);
                            }
                        }
                    }
                }
            }
            else
            {
                if (context.Configuration is ConfigurationRoot root)
                {
                    sections.Add(section);
                }
            }

            foreach (string section in sections)
            {
                if (context.Properties.TryGetValue(section, out object? value))
                {
                    if (value is List<Type> configurations)
                    {
                        if (configurations.Contains(typeof(TConfiguration)))
                        {
                            return;
                        }
                        else
                        {
                            configurations.Add(typeof(TConfiguration));
                        }
                    }
                }
                else
                {
                    context.Properties.Add(section, new List<Type> { typeof(TConfiguration) });
                }

                services.AddKeyedScoped<IConfigurationCache, ConfigurationCache>(section);

                services.TryAddKeyedTransient<IConfigurationFile<TConfiguration>>(section, (provider, key) =>
                {
                    IFileInfo? fileInfo = null;
                    if (provider.GetService<IHostEnvironment>() is IHostEnvironment hostEnvironment)
                    {
                        Microsoft.Extensions.FileProviders.IFileProvider fileProvider = hostEnvironment.ContentRootFileProvider;
                        fileInfo = fileProvider.GetFileInfo(path);
                    }

                    fileInfo ??= new PhysicalFileInfo(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path)));
                    return new ConfigurationFile<TConfiguration>(fileInfo);
                });

                services.TryAddKeyedTransient<IConfigurationSource<TConfiguration>>(section, (provider, key) =>
                {
                    JsonSerializerOptions? defaultSerializer = null;
                    if (serializerDelegate is not null)
                    {
                        defaultSerializer = new JsonSerializerOptions();
                        serializerDelegate.Invoke(defaultSerializer);
                    }

                    return new ConfigurationSource<TConfiguration>(provider.GetRequiredKeyedService<IConfigurationFile<TConfiguration>>(section),
                        section,
                        provider.GetRequiredKeyedService<IConfigurationCache>(section),
                        defaultSerializer);
                });

                services.TryAddKeyedTransient<IConfigurationReader<TConfiguration>>(section, (provider, key) =>
                    new ConfigurationReader<TConfiguration>(provider.GetRequiredKeyedService<IConfigurationSource<TConfiguration>>(key),
                        provider.GetRequiredKeyedService<IConfigurationFactory<TConfiguration>>(key)));

                services.TryAddKeyedTransient<IConfigurationWriter<TConfiguration>>(section, (provider, key) =>
                    new ConfigurationWriter<TConfiguration>(provider.GetRequiredKeyedService<IConfigurationSource<TConfiguration>>(key),
                        provider.GetRequiredKeyedService<IConfigurationFactory<TConfiguration>>(key)));

                services.TryAddKeyedTransient<IConfigurationFactory<TConfiguration>>(section, (provider, key) =>
                    new ConfigurationFactory<TConfiguration>(() => defaultConfiguration ?? provider.GetRequiredKeyedService<TConfiguration>(key)));

                services.AddTransient<IInitialization, ConfigurationInitializer<TConfiguration>>(provider =>
                    new ConfigurationInitializer<TConfiguration>(provider.GetRequiredKeyedService<IConfigurationReader<TConfiguration>>(section),
                        provider.GetRequiredKeyedService<IConfigurationWriter<TConfiguration>>(section),
                        provider.GetRequiredKeyedService<IConfigurationFactory<TConfiguration>>(section),
                        provider.GetRequiredService<IMessenger>()));

                services.AddTransient<IConfigurationInitializer<TConfiguration>, ConfigurationInitializer<TConfiguration>>(provider =>
                    provider.GetRequiredService<IServiceFactory>().Create<ConfigurationInitializer<TConfiguration>>(section));

                services.TryAddKeyedTransient<IWritableConfiguration<TConfiguration>>(section, (provider, key) =>
                    new WritableConfiguration<TConfiguration>(provider.GetRequiredKeyedService<IConfigurationWriter<TConfiguration>>(key)));

                services.TryAddTransient<IWritableConfiguration<TConfiguration>>(provider =>
                    new WritableConfiguration<TConfiguration>(provider.GetRequiredKeyedService<IConfigurationWriter<TConfiguration>>(section)));

                services.TryAddKeyedTransient<IConfigurationDescriptor<TConfiguration>>(section, (provider, key) =>
                    new ConfigurationDescriptor<TConfiguration>(section, provider.GetRequiredKeyedService<IConfigurationReader<TConfiguration>>(key)));

                services.TryAddTransient(provider =>
                    provider.GetRequiredKeyedService<IConfigurationDescriptor<TConfiguration>>(section));

                services.TryAddTransient(provider =>
                    provider.GetRequiredKeyedService<IConfigurationDescriptor<TConfiguration>>(section).Value);

                services.AddHostedService(provider => 
                    new ConfigurationMonitor<TConfiguration>(section,
                        provider.GetRequiredKeyedService<IConfigurationCache>(section),
                        provider.GetRequiredKeyedService<IConfigurationFile<TConfiguration>>(section),
                        provider.GetRequiredService<IServiceProvider>(), 
                        provider.GetRequiredService<IMessenger>()));
            }
        });

        return builder;
    }

    public static IHostBuilder AddSerial<TConfiguration, TReader, TRead, TEvent>(this IHostBuilder hostBuilder)
        where TConfiguration : ISerialConfiguration
        where TReader : SerialReader<TRead>
        where TEvent : SerialEventArgs<TRead>, new()
    {
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
        {
            serviceCollection.TryAddSingleton<ISerialContextFactory, SerialContextFactory>();
            serviceCollection.AddSingleton<ISerialContext<TReader, TRead, TEvent>>(provider => provider.GetRequiredService<ISerialContextFactory>().Create<TConfiguration, TReader, TRead, TEvent>() 
                ?? throw new NullReferenceException());
        });

        return hostBuilder;
    }

    //public static IHostBuilder ConfigureSerials(this IHostBuilder hostBuilder, Action<ISerialBuilder> builderDelegate)
    //{
    //    hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
    //    {
    //        SerialBuilder? builder = new();

    //        builderDelegate.Invoke(builder);
    //        serviceCollection.TryAddSingleton<ISerialContextFactory, SerialContextFactory>();

    //        foreach (ISerialBuilderConfiguration configuration in builder.Configurations)
    //        {
    //            serviceCollection.AddSingleton(provider => configuration.Factory.Invoke(provider) ?? throw new NullReferenceException());
    //        }
    //    });

    //    return hostBuilder;
    //}

    public static IHostBuilder UseContentRoot(this IHostBuilder hostBuilder,
        string contentRoot,
        bool createDirectory)
    {
        if (createDirectory)
        {
            Directory.CreateDirectory(contentRoot);
        }

        hostBuilder.UseContentRoot(contentRoot);
        return hostBuilder;
    }
}