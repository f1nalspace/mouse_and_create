using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MouseAndCreate.Editor
{
    public class IsIntegerConverter : MarkupExtension, IValueConverter
    {
        public bool Invert { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && parameter is int checkValue)
            {
                bool result;
                if (Invert)
                    result = intValue != checkValue;
                else
                    result = intValue == checkValue;
                if (typeof(Visibility).Equals(targetType))
                    return result ? Visibility.Visible : Visibility.Collapsed;
                else
                    return result;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
