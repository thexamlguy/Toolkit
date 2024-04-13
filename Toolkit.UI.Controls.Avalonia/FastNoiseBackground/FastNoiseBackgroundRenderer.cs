using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia;
using SukiUI.Utilities.Background;
using Avalonia.Platform;
using Avalonia.Media;

namespace Toolkit.UI.Controls.Avalonia;

public class FastNoiseBackgroundRenderer
{
    private static readonly FastNoiseLite NoiseGen = new();
    private static readonly Random Rand = new();
    private readonly float accentAlpha;
    private readonly object lockObj = new();
    private readonly float primaryAlpha;
    private readonly float scale;
    private readonly float xSeed;
    private readonly float ySeed;
    private uint accentColour;
    private float aOffsetX;
    private float aOffsetY;
    private uint baseColour;
    private bool isRedrawing;
    private float pOffsetX;
    private float pOffsetY;
    private uint themeColour;

    public FastNoiseBackgroundRenderer(FastNoiseRendererOptions? options = null)
    {
        FastNoiseRendererOptions opt = options ?? 
            new FastNoiseRendererOptions(FastNoiseLite.NoiseType.OpenSimplex2);

        NoiseGen.SetNoiseType(opt.Type);
        scale = opt.NoiseScale * 100f;

        xSeed = opt.XSeed;
        ySeed = opt.YSeed;
        primaryAlpha = opt.PrimaryAlpha;
        accentAlpha = opt.AccentAlpha;
    }

    public async void Render(WriteableBitmap bitmap)
    {
        pOffsetX += xSeed;
        pOffsetY += ySeed;
        aOffsetX -= xSeed;
        aOffsetY -= ySeed;

        if (isRedrawing) return;
        lock (lockObj) { isRedrawing = true; }

        await Task.Run(() =>
        {
            using ILockedFramebuffer frameBuffer = bitmap.Lock();
            PixelSize frameSize = frameBuffer.Size;
            float frameScale = 1f / frameSize.Height * scale;
            unsafe
            {
                uint* backBuffer = (uint*)frameBuffer.Address.ToPointer();
                int stride = frameBuffer.RowBytes / 4;

                Parallel.For(0, frameSize.Height, (long scanline) =>
                {
                    for (int x = 0; x < frameSize.Width; x++)
                    {
                        float noise = NoiseGen.GetNoise((pOffsetX + x) * frameScale, (pOffsetY + scanline) * frameScale);
                        noise = (noise + 1f) / 2f * primaryAlpha; // noise returns -1 to +1 which isn't useful.
                        byte alpha = (byte)(noise * 255);
                        uint firstLayer = BlendPixelOverlay(WithAlpha(themeColour, alpha), baseColour);

                        noise = NoiseGen.GetNoise((aOffsetX + x) * frameScale, (aOffsetY + scanline) * frameScale);
                        noise = (noise + 1f) / 2f * accentAlpha;
                        alpha = (byte)(noise * 255);

                        (backBuffer + scanline * stride + 0)[x] = BlendPixel(WithAlpha(accentColour, alpha), firstLayer);
                    }
                });
            }
        });

        lock (lockObj) { isRedrawing = false; }
    }

    public void UpdateValues(Color primary,
        Color accent,
        ThemeVariant baseTheme)
    {
        themeColour = ToUInt32(primary);
        accentColour = ToUInt32(accent);

        baseColour = baseTheme == ThemeVariant.Light
            ? new Color(255, 241, 241, 241).ToUInt32()
            : GetBackgroundColour(primary);

        pOffsetX = Rand.Next(1000);
        pOffsetY = Rand.Next(1000);

        aOffsetY = Rand.Next(1000);
        aOffsetX = Rand.Next(1000);
    }

    private static byte A(uint col) => (byte)(col >> 24);

    private static uint ARGB(byte a, byte r, byte g, byte b) =>
        (uint)(a << 24 | r << 16 | g << 8 | b << 0);

    private static byte B(uint col) => (byte)col;

    private static uint BlendPixel(uint fore, uint back)
    {
        float alphaF = A(fore) / 255.0f;

        byte resultR = (byte)(R(fore) * alphaF + R(back) * (1 - alphaF));
        byte resultG = (byte)(G(fore) * alphaF + G(back) * (1 - alphaF));
        byte resultB = (byte)(B(fore) * alphaF + B(back) * (1 - alphaF));
        byte resultA = A(back);

        return ARGB(resultA, resultR, resultG, resultB);
    }

    private static uint BlendPixelOverlay(uint fore, uint back)
    {
        float alphaF = A(fore) / 255.0f;

        byte resultR = OverlayComponentBlend(R(fore), R(back), alphaF);
        byte resultG = OverlayComponentBlend(G(fore), G(back), alphaF);
        byte resultB = OverlayComponentBlend(B(fore), B(back), alphaF);

        return ARGB(A(back), resultR, resultG, resultB);
    }

    private static byte G(uint col) => 
        (byte)(col >> 8);

    private static uint GetBackgroundColour(Color input)
    {
        int r = input.R;
        int g = input.G;
        int b = input.B;

        int minValue = Math.Min(Math.Min(r, g), b);
        int maxValue = Math.Max(Math.Max(r, g), b);

        r = r == minValue ? 30 : r == maxValue ? 30 : 22;
        g = g == minValue ? 30 : g == maxValue ? 30 : 22;
        b = b == minValue ? 30 : b == maxValue ? 30 : 22;
        return ARGB(255, (byte)r, (byte)g, (byte)b);
    }

    private static byte OverlayComponentBlend(byte componentF, byte componentB, float alphaF)
    {
        float result = componentB <= 128
            ? 2 * componentF * componentB / 255.0f
            : 255 - 2 * (255 - componentF) * (255 - componentB) / 255.0f;

        return (byte)(result * alphaF + componentB * (1 - alphaF));
    }

    private static byte R(uint col) => (byte)(col >> 16);

    private static uint ToUInt32(Color colour) => 
        (uint)(colour.A << 24 | colour.R << 16 | colour.G << 8 | colour.B);

    private static uint WithAlpha(uint col, byte a) => col & 0x00FFFFFF | (uint)(a << 24);
}