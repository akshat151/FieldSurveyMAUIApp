using System;
using System.Globalization;
using System.Linq;
using FieldSurveyMAUIApp.Models;
using Microsoft.Maui.Controls;

namespace FieldSurveyMAUIApp.Converters
{
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

    public class StringToChoiceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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