using System;
using System.Globalization;
using System.Windows.Data;

namespace Ping.Converters
{
    class ServerTypeNameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var serversType = (string)value;

            switch (serversType)
            {
                default:
                case "League of Legends":
                    return "Images/lol-logo.png";
                case "Playerunknown's Battlegrounds":
                    return "Images/pubg-logo.png";
                case "Counter-Strike: Global Offensive":
                    return "Images/csgo-logo.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
