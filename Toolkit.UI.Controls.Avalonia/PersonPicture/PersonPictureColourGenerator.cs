using Avalonia.Media;
using System.Security.Cryptography;
using System.Text;

namespace Toolkit.UI.Controls.Avalonia;

public class PersonPictureColourGenerator
{
    private readonly string[] colours =
    [
        "#FFB900", "#E81123", "#0078D7", "#0099BC", "#7A7574",
        "#767676", "#FF8C00", "#E81123", "#0063B1", "#2D7D9A",
        "#5D5A58", "#4C4A48", "#F7630C", "#EA005E", "#8E8CD8",
        "#0078D7", "#68768A", "#69797E", "#CA5010", "#C30052",
        "#6B69D6", "#038387", "#515C6B", "#4A5459", "#DA3B01",
        "#E3008C", "#8764B8", "#00B294", "#567C73", "#647C64"
    ];

    public Color GenerateColour(string input)
    {
        byte[] hashBytes = GetHash(input);
        int colourIndex = BitConverter.ToInt32(hashBytes, 0) % colours.Length;
        colourIndex = Math.Abs(colourIndex);

        return HexToColour(colours[colourIndex]);
    }

    private byte[] GetHash(string input)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(input));
    }

    private Color HexToColour(string hex)
    {
        hex = hex.Replace("#", string.Empty);

        byte a = 255;
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return Color.FromArgb(a, r, g, b);
    }
}
