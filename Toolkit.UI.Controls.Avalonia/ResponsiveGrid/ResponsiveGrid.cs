using Avalonia;
using Avalonia.Controls;

namespace Toolkit.UI.Controls.Avalonia
{
    public class ResponsiveGrid : Grid
    {
        public static readonly AvaloniaProperty<int> ActualColumnProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("ActualColumn", 0);

        public static readonly AvaloniaProperty<int> ActualRowProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("ActualRow", 0);

        public static readonly StyledProperty<SizeThresholds> BreakPointsProperty =
            AvaloniaProperty.Register<ResponsiveGrid, SizeThresholds>(nameof(Thresholds));

        public static readonly AvaloniaProperty<int> LargeOffsetProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("LargeOffset", 0);

        public static readonly AvaloniaProperty<int> LargePullProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("LargePull", 0);

        public static readonly AvaloniaProperty<int> LargePushProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("LargePush", 0);

        public static readonly AvaloniaProperty<int> LargeProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("Large", 0);

        public static readonly StyledProperty<int> MaxDivisionProperty =
            AvaloniaProperty.Register<ResponsiveGrid, int>(nameof(MaxDivision), 12);

        public static readonly AvaloniaProperty<int> MediumOffsetProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("MediumOffset", 0);

        public static readonly AvaloniaProperty<int> MediumPullProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("MediumPull", 0);

        public static readonly AvaloniaProperty<int> MediumPushProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("MediumPush", 0);

        public static readonly AvaloniaProperty<int> MediumProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("Medium", 0);

        public static readonly AvaloniaProperty<int> SmallOffsetProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("SmallOffset", 0);

        public static readonly AvaloniaProperty<int> SmallPullProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("SmallPull", 0);

        public static readonly AvaloniaProperty<int> SmallPushProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("SmallPush", 0);

        public static readonly AvaloniaProperty<int> SmallProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("Small", 0);

        public static readonly AvaloniaProperty<int> ExtraSmallOffsetProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("ExtraSmallOffset", 0);

        public static readonly AvaloniaProperty<int> ExtraSmallPullProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("ExtraSmallPull", 0);

        public static readonly AvaloniaProperty<int> ExtraSmallPushProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("ExtraSmallPush", 0);

        public static readonly AvaloniaProperty<int> ExtraSmallProperty =
            AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("ExtraSmall", 0);

        static ResponsiveGrid()
        {
            AffectsMeasure<ResponsiveGrid>(
                MaxDivisionProperty,
                BreakPointsProperty,
                LargeProperty,
                MediumProperty,
                SmallProperty,
                ExtraSmallProperty,
                LargeOffsetProperty,
                LargePullProperty,
                LargePushProperty,
                MediumOffsetProperty,
                MediumPullProperty,
                MediumPushProperty,
                SmallOffsetProperty,
                SmallPullProperty,
                SmallPushProperty,
                ExtraSmallOffsetProperty,
                ExtraSmallPullProperty,
                ExtraSmallPushProperty
            );
        }

        public ResponsiveGrid()
        {
            MaxDivision = 12;
            Thresholds = new SizeThresholds();
        }

        public int MaxDivision
        {
            get => GetValue(MaxDivisionProperty);
            set => SetValue(MaxDivisionProperty, value);
        }

        public SizeThresholds Thresholds
        {
            get => GetValue(BreakPointsProperty);
            set => SetValue(BreakPointsProperty, value);
        }

        public static int GetActualColumn(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(ActualColumnProperty);

        public static int GetActualRow(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(ActualRowProperty);

        public static int GetLarge(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(LargeProperty);

        public static int GetLargeOffset(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(LargeOffsetProperty);

        public static int GetLargePull(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(LargePullProperty);

        public static int GetLargePush(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(LargePushProperty);

        public static int GetMedium(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(MediumProperty);

        public static int GetMediumOffset(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(MediumOffsetProperty);

        public static int GetMediumPull(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(MediumPullProperty);

        public static int GetMediumPush(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(MediumPushProperty);

        public static int GetSmall(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(SmallProperty);

        public static int GetSmallOffset(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(SmallOffsetProperty);

        public static int GetSmallPull(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(SmallPullProperty);

        public static int GetSmallPush(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(SmallPushProperty);

        public static int GetExtraSmall(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(ExtraSmallProperty);

        public static int GetExtraSmallOffset(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(ExtraSmallOffsetProperty);

        public static int GetExtraSmallPull(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(ExtraSmallPullProperty);

        public static int GetExtraSmallPush(AvaloniaObject avaloniaObject) =>
            avaloniaObject.GetValue<int>(ExtraSmallPushProperty);

        public static void SetLarge(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(LargeProperty, value);

        public static void SetLargeOffset(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(LargeOffsetProperty, value);

        public static void SetLargePull(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(LargePullProperty, value);

        public static void SetLargePush(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(LargePushProperty, value);

        public static void SetMedium(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(MediumProperty, value);

        public static void SetMediumOffset(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(MediumOffsetProperty, value);

        public static void SetMediumPull(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(MediumPullProperty, value);

        public static void SetMediumPush(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(MediumPushProperty, value);

        public static void SetSmall(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(SmallProperty, value);

        public static void SetSmallOffset(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(SmallOffsetProperty, value);

        public static void SetSmallPull(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(SmallPullProperty, value);

        public static void SetSmallPush(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(SmallPushProperty, value);

        public static void SetExtraSmall(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(ExtraSmallProperty, value);

        public static void SetExtraSmallOffset(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(ExtraSmallOffsetProperty, value);

        public static void SetExtraSmallPull(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(ExtraSmallPullProperty, value);

        public static void SetExtraSmallPush(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(ExtraSmallPushProperty, value);

        protected static void SetActualColumn(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(ActualColumnProperty, value);

        protected static void SetActualRow(AvaloniaObject avaloniaObject, int value) =>
            avaloniaObject.SetValue(ActualRowProperty, value);

        protected override Size ArrangeOverride(Size finalSize)
        {
            double columnWidth = finalSize.Width / MaxDivision;

            IEnumerable<IGrouping<int, Control>> groupedRows = Children.OfType<Control>().GroupBy(GetActualRow);

            double yOffset = 0;
            foreach (IGrouping<int, Control> row in groupedRows)
            {
                double maxRowHeight = row.Max(control => control.DesiredSize.Height);

                foreach (Control element in row)
                {
                    int column = GetActualColumn(element);
                    int span = GetSpan(element, finalSize.Width);

                    Rect rect = new(column * columnWidth, yOffset, span * columnWidth, maxRowHeight);
                    element.Arrange(rect);
                }

                yOffset += maxRowHeight;
            }

            return finalSize;
        }

        protected int GetOffset(Control control, double width)
        {
            int GetXS(Control control) => GetExtraSmallOffset(control) is 0 ? 0 : GetExtraSmallOffset(control);
            int GetSM(Control control) => GetSmallOffset(control) is 0 ? GetXS(control) : GetSmallOffset(control);
            int GetMD(Control control) => GetMediumOffset(control) is 0 ? GetSM(control) : GetMediumOffset(control);
            int GetLG(Control control) => GetLargeOffset(control) is 0 ? GetMD(control) : GetLargeOffset(control);

            int span = width < Thresholds.ExtraSmallToSmall ? GetXS(control) : width < Thresholds.SmallToMedium ?
                GetSM(control) : width < Thresholds.MediumToLarge ? GetMD(control) : GetLG(control);
            return Math.Min(span, MaxDivision);
        }

        protected int GetPull(Control control, double width)
        {
            int GetXS(Control control) => GetExtraSmallPull(control) is 0 ? 0 : GetExtraSmallPull(control);
            int GetSM(Control control) => GetSmallPull(control) is 0 ? GetXS(control) : GetSmallPull(control);
            int GetMD(Control control) => GetMediumPull(control) is 0 ? GetSM(control) : GetMediumPull(control);
            int GetLG(Control control) => GetLargePull(control) is 0 ? GetMD(control) : GetLargePull(control);

            int span = width < Thresholds.ExtraSmallToSmall ? GetXS(control) : width < Thresholds.SmallToMedium ?
                GetSM(control) : width < Thresholds.MediumToLarge ? GetMD(control) : GetLG(control);
            return Math.Min(span, MaxDivision);
        }

        protected int GetPush(Control control, double width)
        {
            int GetXS(Control control) => GetExtraSmallPush(control) is 0 ? 0 : GetExtraSmallPush(control);
            int GetSM(Control control) => GetSmallPush(control) is 0 ? GetXS(control) : GetSmallPush(control);
            int GetMD(Control control) => GetMediumPush(control) is 0 ? GetSM(control) : GetMediumPush(control);
            int GetLG(Control control) => GetLargePush(control) is 0 ? GetMD(control) : GetLargePush(control);

            int span = width < Thresholds.ExtraSmallToSmall ? GetXS(control) : width < Thresholds.SmallToMedium ?
                GetSM(control) : width < Thresholds.MediumToLarge ? GetMD(control) : GetLG(control);

            return Math.Min(span, MaxDivision);
        }

        protected int GetSpan(Control control, double width)
        {
            int GetXS(Control control) => GetExtraSmall(control) is 0 ? MaxDivision : GetExtraSmall(control);
            int GetSM(Control control) => GetSmall(control) is 0 ? GetXS(control) : GetSmall(control);
            int GetMD(Control control) => GetMedium(control) is 0 ? GetSM(control) : GetMedium(control);
            int GetLG(Control control) => GetLarge(control) is 0 ? GetMD(control) : GetLarge(control);

            int span = width < Thresholds.ExtraSmallToSmall ? GetXS(control) : width < Thresholds.SmallToMedium ?
                GetSM(control) : width < Thresholds.MediumToLarge ? GetMD(control) : GetLG(control);

            return Math.Min(Math.Max(0, span), MaxDivision); ;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            int count = 0;
            int currentRow = 0;

            double availableWidth = double.IsPositiveInfinity(availableSize.Width)
                ? double.PositiveInfinity
                : availableSize.Width / MaxDivision;

            foreach (Control control in Children.OfType<Control>())
            {
                if (control.IsVisible)
                {
                    int span = GetSpan(control, availableSize.Width);
                    int offset = GetOffset(control, availableSize.Width);
                    int push = GetPush(control, availableSize.Width);
                    int pull = GetPull(control, availableSize.Width);

                    if (count + span + offset > MaxDivision)
                    {
                        currentRow++;
                        count = 0;
                    }

                    SetActualColumn(control, count + offset + push - pull);
                    SetActualRow(control, currentRow);

                    count += span + offset;

                    Size size = new(availableWidth * span, double.PositiveInfinity);
                    control.Measure(size);
                }
            }

            IEnumerable<IGrouping<int, Control>> groupedRows = Children.OfType<Control>().GroupBy(GetActualRow);

            Size totalSize = new(groupedRows.Max(rows => rows.Sum(control => control.DesiredSize.Width)),
                groupedRows.Sum(rows => rows.Max(control => control.DesiredSize.Height)));

            return totalSize;
        }
    }
}