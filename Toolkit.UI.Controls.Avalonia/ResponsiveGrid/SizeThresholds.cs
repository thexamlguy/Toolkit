using Avalonia;
using System.ComponentModel;

namespace Toolkit.UI.Controls.Avalonia
{
    [TypeConverter(typeof(SizeThresholdsTypeConverter))]
    public class SizeThresholds : AvaloniaObject
    {
        public static readonly StyledProperty<double> MediumToLargeProperty =
            AvaloniaProperty.Register<SizeThresholds, double>("MediumToLarge", 1200.0);

        public static readonly StyledProperty<double> SmallToMediumProperty =
            AvaloniaProperty.Register<SizeThresholds, double>("SmallToMedium", 992.0);

        public static readonly StyledProperty<double> ExtraSmallToSmallProperty =
            AvaloniaProperty.Register<SizeThresholds, double>("ExtraSmallToSmall", 768.0);

        public double MediumToLarge
        {
            get => (double)GetValue(MediumToLargeProperty);
            set => SetValue(MediumToLargeProperty, value);
        }

        public double SmallToMedium
        {
            get => (double)GetValue(SmallToMediumProperty);
            set => SetValue(SmallToMediumProperty, value);
        }

        public double ExtraSmallToSmall
        {
            get => (double)GetValue(ExtraSmallToSmallProperty);
            set => SetValue(ExtraSmallToSmallProperty, value);
        }
    }
}
