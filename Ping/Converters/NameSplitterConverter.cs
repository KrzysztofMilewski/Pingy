using System;
using System.Globalization;
using System.Windows.Data;

namespace Ping.Converters
{
    public class NameSplitterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var serverTypeName = (string)value;

            if (serverTypeName.Length < 20)
                return serverTypeName;
            else
                return serverTypeName.Substring(0, serverTypeName.IndexOf(' ')) + "\r\n" + serverTypeName.Substring(serverTypeName.IndexOf(' ') + 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
