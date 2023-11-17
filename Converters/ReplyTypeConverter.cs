using System;
using System.Globalization;
using System.Windows.Data;

namespace HelpHive.Converters
{
    public class ReplyTypeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !(values[0] is string) || !(values[1] is bool))
                return string.Empty;

            string postedBy = (string)values[0];
            bool isAdminReply = (bool)values[1];

            return isAdminReply ? $"{postedBy} (Staff Reply)" : $"{postedBy} (User Reply)";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}