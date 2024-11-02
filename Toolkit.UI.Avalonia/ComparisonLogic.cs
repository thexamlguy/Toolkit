using Avalonia.Xaml.Interactivity;
using System.ComponentModel;
using System.Globalization;

namespace Toolkit.UI.Avalonia;

internal static class ComparisonLogic
{
    internal static bool Evaluate(object leftOperand,
        ComparisonConditionType operatorType,
        object? rightOperand)
    {
        bool result = false;

        if (leftOperand != null)
        {
            Type leftType = leftOperand.GetType();

            if (rightOperand != null)
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(leftType);
                rightOperand = typeConverter.ConvertFrom(rightOperand);
            }
        }

        if (leftOperand is IComparable leftComparableOperand &&
            rightOperand is IComparable rightComparableOperand)
        {
            return EvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
        }

        switch (operatorType)
        {
            case ComparisonConditionType.Equal:
                result = Equals(leftOperand, rightOperand);
                break;

            case ComparisonConditionType.NotEqual:
                result = !Equals(leftOperand, rightOperand);
                break;
        }
        return result;
    }

    private static bool EvaluateComparable(IComparable leftOperand,
        ComparisonConditionType operatorType,
        IComparable rightOperand)
    {
        object? convertedOperand = null;

        try
        {
            convertedOperand = Convert.ChangeType(rightOperand, leftOperand.GetType(), CultureInfo.CurrentCulture);
        }
        catch (FormatException)
        {
        }
        catch (InvalidCastException)
        {
        }

        if (convertedOperand == null)
        {
            return operatorType == ComparisonConditionType.NotEqual;
        }

        int comparison = leftOperand.CompareTo((IComparable)convertedOperand);
        bool result = false;

        switch (operatorType)
        {
            case ComparisonConditionType.Equal:
                result = comparison == 0;
                break;

            case ComparisonConditionType.GreaterThan:
                result = comparison > 0;
                break;

            case ComparisonConditionType.GreaterThanOrEqual:
                result = comparison >= 0;
                break;

            case ComparisonConditionType.LessThan:
                result = comparison < 0;
                break;

            case ComparisonConditionType.LessThanOrEqual:
                result = comparison <= 0;
                break;

            case ComparisonConditionType.NotEqual:
                result = comparison != 0;
                break;
        }

        return result;
    }
}