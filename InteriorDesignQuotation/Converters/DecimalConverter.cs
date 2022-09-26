using System;
using System.Globalization;
using System.Windows.Data;

namespace InteriorDesignQuotation.Converters;

public class DecimalConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var input = value?.ToString();
        return !string.IsNullOrWhiteSpace(input) && decimal.TryParse(input, out var result) ? result : null;
    }

    public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var input = value?.ToString();
        var result = !string.IsNullOrWhiteSpace(input) ? input : null;
        return result;
    }
}