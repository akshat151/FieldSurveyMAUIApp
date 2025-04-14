using System;
using System.Globalization;
using System.Linq;
using FieldSurveyMAUIApp.Models;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.Converters
{
    /// <summary>
    /// Converts a string value to a boolean indicating whether the string is not empty or whitespace.
    /// Used to enable/disable controls or show/hide UI elements based on string content.
    /// </summary>
    public class StringNotEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return !string.IsNullOrWhiteSpace(stringValue);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts a boolean value to FontAttributes.
    /// When true, returns Bold; otherwise returns None.
    /// Used to highlight required fields in the UI.
    /// </summary>
    public class BoolToFontAttributesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isRequired && isRequired)
            {
                return FontAttributes.Bold;
            }
            return FontAttributes.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts between string and DateTime values.
    /// Used for date fields in survey forms to handle date formatting and parsing.
    /// If string parsing fails, defaults to today's date.
    /// </summary>
    public class StringToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && !string.IsNullOrEmpty(stringValue))
            {
                if (DateTime.TryParse(stringValue, out DateTime result))
                {
                    return result;
                }
            }
            return DateTime.Today;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateValue)
            {
                return dateValue.ToString("yyyy-MM-dd");
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// Converts between string values and Choice objects.
    /// Used for dropdown/picker controls to match string values with their corresponding Choice objects.
    /// The parameter must be a collection of Choice objects to search through.
    /// </summary>
    public class StringToChoiceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string selectedValue && parameter is System.Collections.Generic.IEnumerable<Choice> choices)
            {
                return choices.FirstOrDefault(c => c.Value == selectedValue);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Choice choice)
            {
                return choice.Value;
            }
            return string.Empty;
        }
    }
}