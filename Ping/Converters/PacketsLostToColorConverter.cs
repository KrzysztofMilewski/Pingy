using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Ping.Converters
{
    public class PacketsLostToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int packetsLost = (int)value;

            if (packetsLost == 0)
                return Brushes.Green;
            else
                return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
