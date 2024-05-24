using System.ComponentModel;
using System.Globalization;

namespace Toolkit.UI.Controls.Avalonia
{
    public class SizeThresholdsTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext? context,
            CultureInfo? culture, object value)
        {
            if (value is not string text)
            {
                return new SizeThresholds();
            }

            List<int> values = text.Split(',')
                             .Select(o => o.Trim())
                             .Select(o => int.TryParse(o, out var result) ? result : 0)
                             .ToList();

            if (values.Count != 3)
            {
                return new SizeThresholds
                {
                    ExtraSmallToSmall = 768,
                    SmallToMedium = 992,
                    MediumToLarge = 1200
                };
            }

            return new SizeThresholds
            {
                ExtraSmallToSmall = values[0],
                SmallToMedium = values[1],
                MediumToLarge = values[2]
            };
        }
    }
}