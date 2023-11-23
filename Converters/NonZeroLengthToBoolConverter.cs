using System;
using System.Globalization;
using System.Windows.Data;

namespace HelpHive.Converters
{
    // to convert an int value to a boolean value
    public class NonZeroLengthToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                int length = (int)value;
                return length > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}