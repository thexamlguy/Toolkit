using Avalonia.Media;
using System.Security.Cryptography;
using System.Text;

namespace Toolkit.UI.Controls.Avalonia;

public class PersonPictureColourGenerator
{
    private readonly float hue;
    private readonly float saturation;
    private readonly float lightness;
    private const float minLightness = 0.2f;
    private const float maxLightness = 0.8f;

    public PersonPictureColourGenerator(float hue = 210, 
        float saturation = 0.8f,
        float lightness = 0.6f)
    {
        this.hue = hue;
        this.saturation = saturation;
        this.lightness = lightness;
    }

    public Color Create(string input)
    {
        byte[] hashBytes = GetHash(input);

        float h = (hashBytes[0] + hue) % 360;
        float s = (hashBytes[1] / 255.0f) * saturation;
        float l = EnsureNonExtremeLightness((hashBytes[2] / 255.0f) * lightness);

        (byte r, byte g, byte b) = HslToRgb(h, s, l);

        return Color.FromRgb(r, g, b);
    }

    private byte[] GetHash(string input) => 
        SHA256.HashData(Encoding.UTF8.GetBytes(input));

    private float EnsureNonExtremeLightness(float calculatedLightness) => 
        calculatedLightness < minLightness ? minLightness : calculatedLightness > maxLightness ?
        maxLightness : calculatedLightness;

    private (byte r, byte g, byte b) HslToRgb(float h, float s, float l)
    {
        h /= 360;
        float r, g, b;

        if (s == 0)
        {
            r = g = b = l;
        }
        else
        {
            float q = l < 0.5f ? l * (1 + s) : l + s - l * s;
            float p = 2 * l - q;

            r = HueToRgb(p, q, h + 1.0f / 3.0f);
            g = HueToRgb(p, q, h);
            b = HueToRgb(p, q, h - 1.0f / 3.0f);
        }

        return ((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
    }

    private float HueToRgb(float p, float q, float t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1.0f / 6.0f) return p + (q - p) * 6 * t;
        if (t < 1.0f / 2.0f) return q;
        if (t < 2.0f / 3.0f) return p + (q - p) * (2.0f / 3.0f - t) * 6;
        return p;
    }
}
