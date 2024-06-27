using Avalonia.Media;
using System.Security.Cryptography;
using System.Text;

namespace Toolkit.UI.Controls.Avalonia;

public class PersonPictureColourGenerator
{
    private readonly string[] colours =
    [
        "#FFB900", "#FF8C00", "#F7630C", "#CA5010", "#DA3B01", "#EF6950", "#D13438", "#FF4343", 
        "#E74856", "#E81123", "#EA005E", "#C30052", "#E3008C", "#BF0077", "#C239B3", "#9A0089", 
        "#0078D7", "#0063B1", "#8E8CD8", "#6B69D6", "#8764B8", "#744DA9", "#B146C2", "#881798", 
        "#0099BC", "#2D7D9A", "#00B7C3", "#038387", "#00B294", "#018574", "#00CC6A", "#10893E", 
        "#7A7574", "#5D5A58", "#68768A", "#515C6B", "#567C73", "#486860", "#498205", "#107C10", 
        "#767676", "#4C4A48", "#69797E", "#4A5459", "#647C64", "#525E54", "#847545", "#7E735F"
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
