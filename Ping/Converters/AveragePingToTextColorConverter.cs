using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Ping.Converters
{
    public class AveragePingToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double averagePing = (double)value;

            if (averagePing < 100)
                return Brushes.Green;
            else if (averagePing > 100 && averagePing < 250)
                return Brushes.DarkOrange;
            else
                return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
