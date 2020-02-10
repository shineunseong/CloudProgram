using System;
using System.Globalization;
using System.Windows.Data;

namespace MediCloudDrive.Models.Converters
{
    internal class BoolToConnectStringConvertors : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strConn = "좋음";
            if (value is bool == true)
            {
                bool bConn = (bool)value;
                if (bConn == false)
                {
                    strConn = "끊김";
                }
            }
            return strConn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}