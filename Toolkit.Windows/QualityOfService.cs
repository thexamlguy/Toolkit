using System.Runtime.Versioning;

namespace Toolkit.Windows;

[SupportedOSPlatform("windows8.0")]
public enum QualityOfService
{
    Default,
    [SupportedOSPlatform("windows10.0.16299.0")]
    High,
    [SupportedOSPlatform("windows10.0.16299.0")]
    Medium,
    [SupportedOSPlatform("windows10.0.16299.0")]
    Low,
    [SupportedOSPlatform("windows11.0.22621.0")]
    Utility,
    [SupportedOSPlatform("windows11.0")]
    Eco,
    [SupportedOSPlatform("windows10.0.19041.0")]
    Media,
    [SupportedOSPlatform("windows10.0.19041.0")]
    Deadline
}
