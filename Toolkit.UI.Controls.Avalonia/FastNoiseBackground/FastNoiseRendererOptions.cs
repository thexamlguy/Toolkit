namespace SukiUI.Utilities.Background;

public readonly struct FastNoiseRendererOptions(
    FastNoiseLite.NoiseType type,
    float noiseScale = 1.5f,
    float xSeed = 2f,
    float ySeed = 1f,
    float primaryAlpha = 0.7f,
    float accentAlpha = 0.04f,
    float seedScale = 0.1f)
{
    public float AccentAlpha { get; } = accentAlpha;

    public float NoiseScale { get; } = noiseScale;

    public float PrimaryAlpha { get; } = primaryAlpha;

    public FastNoiseLite.NoiseType Type { get; } = type;

    public float XSeed { get; } = xSeed * seedScale;

    public float YSeed { get; } = ySeed * seedScale;
}